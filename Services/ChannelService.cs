using Microsoft.EntityFrameworkCore;
using Ynov_WorkShare_Server.Context;
using Ynov_WorkShare_Server.Interfaces;
using Ynov_WorkShare_Server.Models;

namespace Ynov_WorkShare_Server.Services;

public class ChannelService : IChannelService
{
    private readonly WorkShareDbContext _context;
    private readonly IUserChannelService _iuc;

    public ChannelService(WorkShareDbContext context, IUserChannelService userChannelService)
    {
        _context = context;
        _iuc = userChannelService;
    }
    
    public async Task<IEnumerable<Channel>> GetChannelsByUser(Guid userId)
    {
        if (!_context.Users.AsNoTracking().Any(u => u.Id == userId))
            throw new KeyNotFoundException("Utilisateur introuvable !");

        var channelsId = await _iuc.GetByUser(userId);
        var channels = new List<Channel>();
        
        foreach (var channelId in channelsId)
        {
            var channel = await _context.Channels.AsNoTracking().
                Include(c => c.Messages).
                Include(c => c.Files).
                Include(c => c.UserChannels).
                FirstOrDefaultAsync(c => c.Id == channelId);
            
            if (channel is null) throw new KeyNotFoundException("Canal introuvable!");
            
            channels.Add(channel);
        }
        return channels;
    }

    public async Task<Channel> Get(Guid id)
    {
        var channel = await _context.Channels.AsNoTracking().
            Include(c => c.Messages).
            Include(c => c.Files).
            Include(c => c.UserChannels).
            FirstOrDefaultAsync(c => c.Id == id);
        if (channel is null) throw new KeyNotFoundException("Canal introuvable!");

        return channel;
    }

    public async Task<Channel> Post(Channel channel)
    {
        if (!_context.Users.Any(u => u.Id == channel.AdminId)) throw new KeyNotFoundException("Utilisateur introuvable !");
        
        using var transaction = await _context.Database.BeginTransactionAsync();
        _context.Channels.Add(channel);
        await _context.SaveChangesAsync();
        
        var userChannel = new UserChannel();
        userChannel.UserId = channel.AdminId;
        userChannel.ChannelId = channel.Id;
        await _iuc.Post(userChannel);
        
        transaction.Commit();
        
        return channel;
    }

    public async Task<Channel> Update(Guid id, Channel channel)
    {
        if (!_context.Channels.Any(c => c.Id == id)) throw new KeyNotFoundException("Canal introuvable !");
        
        channel.UpdatedAt = DateTime.UtcNow;
        _context.Entry(channel).State = EntityState.Modified;
        _context.Entry(channel).Property(c => c.CreatedAt).IsModified = false;
        _context.Entry(channel).Property(c => c.AdminId).IsModified = false;

        await _context.SaveChangesAsync();

        return channel;
    }
    
    public async Task<Channel> ChangeAdmin(Guid id, Guid adminId)
    {
        if (!_context.Users.Any(u => u.Id == adminId)) throw new KeyNotFoundException("Utilisateur introuvable !");
        
        var channel = await _context.Channels.AsNoTracking().
            Include(c => c.Messages).
            Include(c => c.Files).
            Include(c => c.UserChannels).
            FirstOrDefaultAsync(c => c.Id == id);
        if (channel is null) throw new KeyNotFoundException("Canal introuvable!");

        channel.AdminId = adminId;
        _context.Channels.Update(channel);
        await _context.SaveChangesAsync();

        return channel;
    }

    public async Task Delete(Guid id)
    {
        var channel = await _context.Channels.AsNoTracking().
            Include(c => c.Messages).
            Include(c => c.Files).
            Include(c => c.UserChannels).
            FirstOrDefaultAsync(c => c.Id == id);
        if (channel is null) throw new KeyNotFoundException("Canal introuvable!");

        _context.Channels.Remove(channel);
        await _context.SaveChangesAsync();
    }
}