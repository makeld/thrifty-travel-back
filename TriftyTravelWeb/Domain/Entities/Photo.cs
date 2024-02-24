namespace Domain.Entities;

public class Photo : BaseEntity
{
    public Guid? ExpenseId { get; set; }
    public Expense? Expense { get; set; }
    
    public Guid? TripId { get; set; }
    public Trip? Trip { get; set; }
    
    public string ImageUrl { get; set; } = default!;
    public string? Description { get; set; } = default!;
}