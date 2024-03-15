using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Ynov_WorkShare_Server.Models;

public class Message
{
    [Key]
    public Guid Id { get; set; }
    
    public Guid UserId { get; set; }
    
    public Guid ChannelId { get; set; }
    
    [MaxLength(500)]
    public string  Content { get; set; } = String.Empty;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set;} = DateTime.UtcNow;
    
    //--------- Relations -----------
    
    
    public virtual User? Author { get; set; }
    
    [JsonIgnore]
    public virtual Channel? Channel { get; set; }
}