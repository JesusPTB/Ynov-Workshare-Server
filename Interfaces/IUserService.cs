using Ynov_WorkShare_Server.DTO;
using Ynov_WorkShare_Server.Models;

namespace Ynov_WorkShare_Server.Interfaces;

public interface IUserService
{
    Task<IEnumerable<UserDto>> GetAll();
    Task<IEnumerable<UserDto>> GetByChannel(Guid channelId);
    
    Task<UserDto> GetById(Guid id);
    
    Task<UserDto> GetByEmail(string email);

    Task<UserDto> Post(User user);

    Task<UserDto> Update(Guid id, User user);

    Task Delete(Guid id);

}