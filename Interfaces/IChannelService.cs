using Ynov_WorkShare_Server.Models;

namespace Ynov_WorkShare_Server.Interfaces;

public interface IChannelService
{
    Task<IEnumerable<Channel>> GetChannelsByUser(Guid userId);

    Task<Channel> Get(Guid id);

    Task<Channel> Post(Channel channel);

    Task<Channel> Update(Guid id, Channel channel);

    Task Delete(Guid id);

    Task<Channel> ChangeAdmin(Guid id, string adminId);
}