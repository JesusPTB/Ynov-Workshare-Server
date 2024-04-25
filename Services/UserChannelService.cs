using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Ynov_WorkShare_Server.Context;
using Ynov_WorkShare_Server.Interfaces;
using Ynov_WorkShare_Server.Models;

namespace Ynov_WorkShare_Server.Services;

public class UserChannelService : IUserChannelService
{
    private readonly WorkShareDbContext _context;
    private readonly UserManager<User> _userManager;

    public UserChannelService(WorkShareDbContext context, UserManager<User> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IEnumerable<Guid>> GetByUser(Guid userId)
    {
        if (!_userManager.Users.AsNoTracking().Any(u => u.Id == userId.ToString()))
            throw new KeyNotFoundException("Utilisateur introuvable !");

        return await _context.UserChannels.Where(uc => uc.UserId == userId.ToString()).Select(uc => uc.ChannelId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Guid>> GetByChannel(Guid channelId)
    {
        if (!_context.Channels.AsNoTracking().Any(c => c.Id == channelId))
            throw new KeyNotFoundException("Canal introuvable !");

        return await _context.UserChannels.Where(uc => uc.ChannelId == channelId).Select(uc => Guid.Parse(uc.UserId))
            .ToListAsync();
    }

    public async Task<UserChannel> GetById(Guid id)
    {
        var userChannel = await _context.UserChannels.FindAsync(id);

        if (userChannel is null) throw new KeyNotFoundException("Utilisateur - Canal introuvable !");

        return userChannel;
    }

    public async Task<UserChannel> Post(UserChannel userChannel)
    {
        if (!_userManager.Users.AsNoTracking().Any(u => u.Id == userChannel.UserId))
            throw new KeyNotFoundException("Utilisateur introuvable !");

        if (!_context.Channels.AsNoTracking().Any(c => c.Id == userChannel.ChannelId))
            throw new KeyNotFoundException("Canal introuvable !");

        _context.UserChannels.Add(userChannel);
        await _context.SaveChangesAsync();
        return userChannel;
    }

    public async Task<UserChannel> SetIsMuted(Guid id, Guid userId, bool mute)
    {
        var userChannel = await _context.UserChannels.AsNoTracking()
            .FirstOrDefaultAsync(uc => uc.ChannelId == id && uc.UserId == userId.ToString());

        if (userChannel is null)
            throw new KeyNotFoundException("Utilisateur introuvable dans ce canal !");

        userChannel.IsMuted = mute;
        _context.UserChannels.Update(userChannel);
        await _context.SaveChangesAsync();
        return userChannel;
    }

    public async Task Remove(Guid id)
    {
        var userChannel = _context.UserChannels.AsNoTracking().FirstOrDefault(uc => uc.Id == id);
        if (userChannel is null)
            throw new KeyNotFoundException("Utilisateur - Canal introuvable !");

        _context.UserChannels.Remove(userChannel);
        await _context.SaveChangesAsync();
    }
}