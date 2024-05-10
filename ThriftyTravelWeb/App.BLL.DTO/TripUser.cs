using Base.Contracts.Domain;

namespace App.BLL.DTO;

public class TripUser: IDomainEntityId
{
    public Guid Id { get; set; }
    public Guid AppUserId { get; set; }
    public Guid TripId { get; set; }
}