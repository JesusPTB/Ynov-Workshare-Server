using System.IdentityModel.Tokens.Jwt;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
    private readonly UserManager<User> _userManager;
    private readonly IConfiguration _config;

    public UserService(WorkShareDbContext context, IUserChannelService userChannelService,
        UserManager<User> userManager, IConfiguration configuration)
    {
        _context = context;
        _iuc = userChannelService;
        _userManager = userManager;
        _config = configuration;
    }

    public async Task<IEnumerable<UserDto>> GetByChannel(Guid channelId)
    {
        if (!_context.Channels.AsNoTracking().Any(c => c.Id == channelId))
            throw new KeyNotFoundException("Canal introuvable !");

        var usersId = await _iuc.GetByChannel(channelId);
        var users = new List<UserDto>();
        
        foreach (var userId in usersId)
        {
            var user = await _userManager.Users.Include(u => u.UserChannels).
                Select(u=> new UserDto(u)).
                FirstOrDefaultAsync(u => u.Id == userId);
            
            if (user is null) throw new KeyNotFoundException("Utilisateur introuvable!");
            
            users.Add(user);
        }
        return users;
    }
    
    public async Task<IEnumerable<UserDto>> GetAll()
    {
        return await _userManager.Users.AsNoTracking().OrderBy(u=>u.Email)
            .Include(u=>u.UserChannels)
            .Select(u => new UserDto(u)).ToListAsync();
    }

    public async Task<UserDto> GetById(Guid id)
    {
        var user = await _userManager.Users.Include(u => u.UserChannels).
            
            FirstOrDefaultAsync(u => u.Id == id.ToString());
        if (user is null) throw new KeyNotFoundException("Utilisateur introuvable !");
        return new UserDto(user);
    }

    public async Task<UserDto> GetByEmail(string email)
    {
        var user = await _userManager.Users.Include(u => u.UserChannels).FirstOrDefaultAsync(u => u.Email == email);
        if (user is null) throw new KeyNotFoundException("Utilisateur introuvable !");
        return new UserDto(user);
    }
    
    
    public async Task<UserDto> Register(UserRegisterForm userForm)
    {
        try
        {
            var result = await _userManager.CreateAsync(userForm.ToUser(), userForm.Password);
            if (!result.Succeeded) throw new Exception(result.ToString());
        }
        catch (Exception ex)
        {
            if (ex.InnerException is PostgresException postgres)
            {
                HandleException(postgres);
            }
            throw new ExternalException(ex.Message);
        }
        return new UserDto(_userManager.Users.AsNoTracking().First(u =>u.Email == userForm.Email));
    }

    public async Task<UserDto> Update(Guid id, User user)
    {

        if (id.ToString() != user.Id) throw new KeyNotFoundException("Utilisateur introuvable !");
        
        var userDb = _userManager.Users.FirstOrDefault(u => u.Id == id.ToString());
        if (userDb is null) throw new KeyNotFoundException("Utilisateur introuvable !");
        
        userDb.UpdatedAt = DateTime.UtcNow;
        userDb.FirstName = user.FirstName;
        userDb.LastName = user.LastName;
        userDb.UserName = user.UserName;
        userDb.Avatar = user.Avatar;
       

        try
        {
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded) throw new Exception(result.ToString());
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
        var user =  _userManager.Users.AsNoTracking().FirstOrDefault(u => u.Id == id.ToString());
        if (user is null) throw new KeyNotFoundException("Utilisateur introuvable !");
        await _userManager.DeleteAsync(user);
    }
    
    public async Task UpdatePassword( UpdatePwdDto form)
    {
        var user = await _userManager.FindByEmailAsync(form.Email);

        if (user is null)
            throw new KeyNotFoundException("Utilisateur introuvable !");

        var result = await _userManager.ChangePasswordAsync(user, form.CurrentPassword, form.NewPassword);

        if (!result.Succeeded) throw new Exception(result.ToString());
                
    }
    
    public async Task<UserDto> Login(LoginForm user)
    {
        var identityUser = await _userManager.FindByEmailAsync(user.Email);
        if (identityUser is null)
            throw new KeyNotFoundException("Email ou mot de passe incorrect");

        if (! await _userManager.CheckPasswordAsync(identityUser, user.Password))
            throw new Exception("Email ou mot de passe incorrect");
            
        return this.GenerateTokenString(user.Email);
    }
    
    private UserDto GenerateTokenString(string email)
    {
        var user = _context.Users.First(u => u.Email == email);
        
        IEnumerable<Claim> claims = new List<Claim> {
            new Claim(ClaimTypes.Email, email)
        };
        var signinKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(
                _config.GetSection("Jwt:Key").Value!
            )
        );
        SigningCredentials signinCred = new (signinKey, SecurityAlgorithms.HmacSha512Signature);
        var securityToken = new JwtSecurityToken(
            claims : claims,
            expires: DateTime.UtcNow.AddMonths(2),
            issuer: _config.GetSection("Jwt:Issuer").Value,
            //audience:_config.GetSection("Jwt:Audience").Value,
            signingCredentials: signinCred
        );
          
        var tokenString = new JwtSecurityTokenHandler().WriteToken(securityToken);

        return new  UserDto(user, tokenString);
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