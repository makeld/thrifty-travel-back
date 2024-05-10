using Base.Contracts.Domain;

namespace App.DAL.DTO;

public class Comment: IDomainEntityId
{
    public Guid Id { get; set; }
    
    public Guid? ParentCommentId { get; set; }
    
    public Guid TripId { get; set; }
    
    public Guid AppUserId { get; set; }
    
    public string Content { get; set; } = default!;
}