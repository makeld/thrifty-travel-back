using Base.Contracts.Domain;
using Domain.Base;
using Domain.Identity;

namespace Domain.Entities;

public class Comment : BaseEntityId, IDomainAppUser<AppUser>
{
    public Guid AppUserId { get; set; }
    public AppUser? AppUser { get; set; }
    
    public Guid TripId { get; set; }
    public Trip? Trip { get; set; }
    
    public Guid? ParentCommentId { get; set; }
    public Comment? ParentComment { get; set; }

    public string Content { get; set; } = default!;
    
    public ICollection<Comment>? ChildComments { get; set; }
}