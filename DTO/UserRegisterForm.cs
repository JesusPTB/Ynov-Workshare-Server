using System.ComponentModel.DataAnnotations;
using Ynov_WorkShare_Server.Models;

namespace Ynov_WorkShare_Server.DTO;

public class UserRegisterForm
{
    [EmailAddress] [MaxLength(50)] public string Email { get; set; } = string.Empty;

    [MaxLength(50)] public string Pseudo { get; set; } = string.Empty;

    [MaxLength(30)] public string FirstName { get; set; } = string.Empty;

    [MaxLength(30)] public string LastName { get; set; } = string.Empty;

    [MaxLength(100)] public string Password { get; set; } = string.Empty;

    public User ToUser()
    {
        return new User
        {
            UserName = Pseudo,
            Email = Email,
            FirstName = FirstName,
            LastName = LastName
        };
    }
}