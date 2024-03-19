using System.ComponentModel.DataAnnotations;

namespace Ynov_WorkShare_Server.Models;

public class Channel
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    public string AdminId { get; set; }= String.Empty;
    
    [MaxLength(30)]
    public string Name { get; set; } = String.Empty;
    
    [MaxLength(150)]
    public string Description { get; set; } = String.Empty;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set;} = DateTime.UtcNow;

    //--------- Relations ------------
    
    public virtual User? Admin { get; set; }
    
    public virtual ICollection<UserChannel>? UserChannels { get; set; }
    
    public virtual ICollection<Message>? Messages { get; set; }
    
    public virtual ICollection<File>? Files { get; set; }
 

}