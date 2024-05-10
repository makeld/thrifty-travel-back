using Base.Contracts.Domain;

namespace App.DAL.DTO;

public class TripLocation: IDomainEntityId
{
    public Guid Id { get; set; }
    public Guid TripId { get; set; }
    public Guid LocationId { get; set; }
}