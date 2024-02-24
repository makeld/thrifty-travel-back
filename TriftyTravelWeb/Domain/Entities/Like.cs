using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

public class Like : BaseEntity
{
    public Guid UserId { get; set; }
    public AppUser? User { get; set; }
    
    public Guid TripId { get; set; }
    public Trip? Trip { get; set; }
    
}