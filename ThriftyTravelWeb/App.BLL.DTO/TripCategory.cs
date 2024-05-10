using Base.Contracts.Domain;

namespace App.BLL.DTO;

public class TripCategory: IDomainEntityId
{
    public Guid Id { get; set; }
    public Guid TripId { get; set; }
    public Guid CategoryId { get; set; }
}