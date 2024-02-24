namespace Domain.Entities;

public class TripLocation : BaseEntity
{
    public Guid TripId { get; set; }
    public Trip? Trip { get; set; }
    
    public Guid LocationId { get; set; }
    public Location? Location { get; set; }

    public ICollection<Expense>? Expenses { get; set; } = new List<Expense>();
}