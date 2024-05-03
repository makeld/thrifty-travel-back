using Domain.Base;

namespace Domain.Entities;

public class TripCategory : BaseEntityId
{
    public Guid TripId { get; set; }
    public Trip? Trip { get; set; }
    
    public Guid CategoryId { get; set; }
    public Category? Category { get; set; }
}