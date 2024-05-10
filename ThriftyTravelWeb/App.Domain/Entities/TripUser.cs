using Base.Contracts.Domain;
using Domain.Base;
using Domain.Identity;
namespace Domain.Entities;

public class TripUser : BaseEntityId, IDomainAppUser<AppUser>
{
    public Guid AppUserId { get; set; }
    public AppUser? AppUser { get; set; }
    
    public Guid TripId { get; set; }
    public Trip? Trip { get; set; }

    public ICollection<UserExpense>? UserExpenses { get; set; }
}