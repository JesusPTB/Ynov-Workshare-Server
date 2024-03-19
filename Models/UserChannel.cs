using System.Text.Json.Serialization;

namespace Ynov_WorkShare_Server.Models;

public class UserChannel
{
    public Guid Id { get; set; }

    public string UserId { get; set; } = String.Empty;
    
    public Guid ChannelId { get; set; }

    public bool IsMuted { get; set; } = false;
    
    public DateTime AddedAt { get; set; } = DateTime.UtcNow;
    
    //--------- Relations -----------
    
    [JsonIgnore]
    public virtual User? User { get; set; }
    
    [JsonIgnore]
    public virtual Channel? Channel { get; set; }
}