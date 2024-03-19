using System.ComponentModel.DataAnnotations;
using Ynov_WorkShare_Server.Models;

namespace Ynov_WorkShare_Server.DTO;

public class UserRegisterForm
{
    [EmailAddress] [MaxLength(50)] 
    public string Email { get; set; } = String.Empty;
    
    [MaxLength(50)]
    public string Pseudo { get; set; } = String.Empty;
    
    [MaxLength(30)]
    public string FirstName { get; set; } = String.Empty;
    
    [MaxLength(30)]
    public string LastName { get; set; } = String.Empty;
    
    [MaxLength(100)]
    public string Password { get; set; } = String.Empty;

    public User ToUser()
    {
        return new User{
            UserName = this.Pseudo,
            Email = this.Email,
            FirstName = this.FirstName,
            LastName = this.LastName,
        };
    }
}