using System.ComponentModel.DataAnnotations;
using Ynov_WorkShare_Server.Models;

namespace Ynov_WorkShare_Server.DTO;

public class UserDto
{
    [Key]
    public Guid Id { get; set; }
    
    [EmailAddress]
    [MaxLength(50)]
    public string Email { get; set; }
    
    [MaxLength(50)]
    public string Pseudo { get; set; }
    
    public string Avatar { get; set; }
    
    
    [MaxLength(30)]
    public string FirstName { get; set; }
    
    [MaxLength(30)]
    public string LastName { get; set; }
    
    public string Token { get; set; } = "";
    
    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set;}
    
    public ICollection<UserChannel>? UserChannels { get; set; }

    public UserDto(User user)
    {
        Id = Guid.Parse(user.Id);
        Email = user.Email!;
        Pseudo = user.UserName!;
        Avatar = user.Avatar;
        FirstName = user.FirstName;
        LastName = user.LastName;
        CreatedAt = user.CreatedAt;
        UpdatedAt = user.UpdatedAt;
        UserChannels = user.UserChannels;
    }
    
    public UserDto(User user, string token)
    {
        Id = Guid.Parse(user.Id);
        Email = user.Email!;
        Pseudo = user.UserName!;
        Avatar = user.Avatar;
        FirstName = user.FirstName;
        LastName = user.LastName;
        CreatedAt = user.CreatedAt;
        UpdatedAt = user.UpdatedAt;
        UserChannels = user.UserChannels;
        Token = token;
    }

    
   
    
}