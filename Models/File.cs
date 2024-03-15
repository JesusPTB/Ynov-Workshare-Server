using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Ynov_WorkShare_Server.Models;

public class File
{
    [Key]
    public Guid Id { get; set; }
    
    public Guid UserId { get; set; }
    
    public Guid ChannelId { get; set; }
    
    
    public FileType  Type { get; set; }
    
    [MaxLength(100)]
    public string  Url { get; set; } = String.Empty;
    
    [MaxLength(50)]
    public string  Name { get; set; } = String.Empty;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set;} = DateTime.UtcNow;
    
    //--------- Relations -----------
    
    
    public virtual User? Author { get; set; }
    
    [JsonIgnore]
    public virtual Channel? Channel { get; set; }
}