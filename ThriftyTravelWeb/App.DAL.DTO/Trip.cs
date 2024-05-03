using Base.Contracts.Domain;

namespace App.DAL.DTO;

public class Trip: IDomainEntityId
{
    public Guid Id { get; set; }
    
    public string TripName { get; set; } = default!;
}
