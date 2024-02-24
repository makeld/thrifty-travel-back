using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

public class TripUser : BaseEntity
{
    public Guid TripId { get; set; }
    public Trip? Trip { get; set; }
    
    public Guid UserId { get; set; }
    public AppUser? User { get; set; }
    
    public ICollection<UserExpense>? Expenses { get; set; } = new List<UserExpense>();
    
}