using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

public class Comment : BaseEntity
{
    public Guid TripId { get; set; }
    public Trip? Trip { get; set; }
    
    public Guid? ParentCommentId { get; set; }
    public Comment? ParentComment { get; set; }
    
    public Guid UserId { get; set; }
    public AppUser? User { get; set; }

    public string Content { get; set; } = default!;

    public ICollection<Comment>? ChildComments { get; set; } = new List<Comment>();

}