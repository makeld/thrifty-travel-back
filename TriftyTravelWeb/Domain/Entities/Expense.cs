namespace Domain.Entities;

public class Expense : BaseEntity
{
    public Guid TripId { get; set; }
    public Trip? Trip { get; set; }
    
    public Guid TripLocationId { get; set; }
    public TripLocation? TripLocation { get; set; }

    public string Type { get; set; } = default!;
    public string Title { get; set; } = default!;
    public DateTime ExpenseDate { get; set; } = DateTime.Now;
    public string? Description { get; set; } = default!;
    public int ExpensePrice { get; set; } = default!;
    public string CurrencyCode { get; set; } = default!;
    
    public ICollection<Photo>? Photos { get; set; } = new List<Photo>();
    public ICollection<UserExpense>? UserExpenses { get; set; } = new List<UserExpense>();
}