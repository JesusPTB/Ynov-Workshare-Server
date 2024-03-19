using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Ynov_WorkShare_Server.Models;

[Index(nameof(Email), IsUnique = true)]
[Index(nameof(UserName), IsUnique = true)]
public class User : IdentityUser
{
    public string Avatar { get; set; } = String.Empty;
    
    [MaxLength(30)]
    public string FirstName { get; set; } = String.Empty;
    
    [MaxLength(30)]
    public string LastName { get; set; } = String.Empty;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set;} = DateTime.UtcNow;
    
    
    //----------- Relations ------------
    [JsonIgnore]
    public virtual ICollection<Channel>? AdministratedChannels { get; set; }
    
    public virtual ICollection<UserChannel>? UserChannels { get; set; }
    
    [JsonIgnore]
    public virtual ICollection<Message>? Messages { get; set; }
    
    [JsonIgnore]
    public virtual ICollection<File>? File { get; set; }

}