using System.ComponentModel.DataAnnotations;

namespace App.DTO.v1_0;

public class Comment
{
    public Guid Id { get; set; }
    
    public Guid? ParentCommentId { get; set; }
    public Comment? ParentComment { get; set; }
    
    public Guid TripId { get; set; }
    public Trip? Trip { get; set; }
    
    public Guid AppUserId { get; set; }
    
    [MaxLength(512)]
    public string Content { get; set; } = default!;
    
    public ICollection<Comment>? ChildComments { get; set; }
}