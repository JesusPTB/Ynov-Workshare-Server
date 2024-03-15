using Ynov_WorkShare_Server.Models;

namespace Ynov_WorkShare_Server.Interfaces;

public interface IUserChannelService
{
    Task<IEnumerable<Guid>> GetByUser(Guid userId);
    Task<IEnumerable<Guid>> GetByChannel(Guid channelId);
    Task<UserChannel> GetById(Guid id);
    Task<UserChannel> Post(UserChannel userChannel);
    Task<UserChannel> SetIsMuted(Guid id, Guid userId, bool mute);
    Task Remove(Guid id);
}