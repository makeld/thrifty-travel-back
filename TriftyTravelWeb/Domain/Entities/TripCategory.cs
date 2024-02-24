namespace Domain.Entities;

public class TripCategory : BaseEntity
{
    public Guid TripId  { get; set; }
    public Trip? Trip  { get; set; }
    
    public Guid CategoryId  { get; set; }
    public Category? Category  { get; set; }
}