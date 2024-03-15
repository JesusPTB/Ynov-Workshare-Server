using System.Runtime.InteropServices;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Ynov_WorkShare_Server.Context;
using Ynov_WorkShare_Server.DTO;
using Ynov_WorkShare_Server.Interfaces;
using Ynov_WorkShare_Server.Models;

namespace Ynov_WorkShare_Server.Services;

public class UserService: IUserService
{
    private readonly WorkShareDbContext _context;
    private readonly IUserChannelService _iuc;

    public UserService(WorkShareDbContext context, IUserChannelService userChannelService)
    {
        _context = context;
        _iuc = userChannelService;
    }

    public async Task<IEnumerable<UserDto>> GetByChannel(Guid channelId)
    {
        if (!_context.Channels.AsNoTracking().Any(c => c.Id == channelId))
            throw new KeyNotFoundException("Canal introuvable !");

        var usersId = await _iuc.GetByChannel(channelId);
        var users = new List<UserDto>();
        
        foreach (var userId in usersId)
        {
            var user = await _context.Users.Include(u => u.UserChannels).
                Select(u=> new UserDto(u)).
                FirstOrDefaultAsync(u => u.Id == userId);
            
            if (user is null) throw new KeyNotFoundException("Utilisateur introuvable!");
            
            users.Add(user);
        }
        return users;
    }
    
    public async Task<IEnumerable<UserDto>> GetAll()
    {
        return await _context.Users.AsNoTracking().OrderBy(u=>u.Email)
            .Include(u=>u.UserChannels)
            .Select(u => new UserDto(u)).ToListAsync();
    }

    public async Task<UserDto> GetById(Guid id)
    {
        var user = await _context.Users.Include(u => u.UserChannels).
            
            FirstOrDefaultAsync(u => u.Id == id);
        if (user is null) throw new KeyNotFoundException("Utilisateur introuvable !");
        return new UserDto(user);
    }

    public async Task<UserDto> GetByEmail(string email)
    {
        var user = await _context.Users.Include(u => u.UserChannels).FirstOrDefaultAsync(u => u.Email == email);
        if (user is null) throw new KeyNotFoundException("Utilisateur introuvable !");
        return new UserDto(user);
    }

    public async Task<UserDto> Post(User user)
    {
        _context.Users.Add(user);
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            if (ex.InnerException is PostgresException postgres)
            {
                HandleException(postgres);
            }
            throw new ExternalException(ex.Message);
        }
        return new UserDto(user);
    }

    public async Task<UserDto> Update(Guid id, User user)
    {
        if (!_context.Users.Any(u => u.Id == id)) throw new KeyNotFoundException("Utilisateur introuvable !");
        
        user.UpdatedAt = DateTime.UtcNow;
        _context.Entry(user).State = EntityState.Modified;
        _context.Entry(user).Property(a => a.CreatedAt).IsModified = false;
        _context.Entry(user).Property(a => a.Email).IsModified = false;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            if (ex.InnerException is PostgresException postgres)
            {
                HandleException(postgres);
            }
            throw new Exception(ex.Message);
        }
        return new UserDto(user);
    }

    public async Task Delete(Guid id)
    {
        var user =  _context.Users.AsNoTracking().FirstOrDefault(u => u.Id == id);
        if (user is null) throw new KeyNotFoundException("Utilisateur introuvable !");
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }
    
    private static void HandleException(PostgresException exception)
    {
        switch (exception.ConstraintName!)
        {
            case "IX_Users_Email":
                throw new BadHttpRequestException("Un compte avec cet email existe déjà.");
            case "IX_Users_Pseudo":
                throw new BadHttpRequestException("Pseudo indisponible.");
        }
    }
}