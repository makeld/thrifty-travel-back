using Base.Contracts.Domain;

namespace App.DAL.DTO;

public class Comment: IDomainEntityId
{
    public Guid Id { get; set; }
    
    public Guid AppUserId { get; set; }
    // public AppUser? AppUser { get; set; }
    
    public Guid TripId { get; set; }
    public Trip? Trip { get; set; }
    
    public Guid? ParentCommentId { get; set; }
    public Comment? ParentComment { get; set; }

    public string Content { get; set; } = default!;
    
    public ICollection<Comment>? ChildComments { get; set; }

}