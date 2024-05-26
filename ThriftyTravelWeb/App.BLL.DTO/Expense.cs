using Base.Contracts.Domain;

namespace App.BLL.DTO;

public class Expense: IDomainEntityId
{
    public Guid Id { get; set; }
    public Guid TripId { get; set; }
    public Trip? Trip { get; set; }
    
    public Guid TripLocationId { get; set; }
    public TripLocation? TripLocation { get; set; }

    public string Title { get; set; } = default!;
    public string Type { get; set; } = default!;
    public string? Description { get; set; } = default!;
    public DateTime ExpenseDate { get; set; }
    public int ExpensePrice { get; set; } = default!;
    public string CurrencyCode { get; set; } = default!;
    
    public ICollection<Photo>? Photos { get; set; }
    public ICollection<UserExpense>? UserExpenses { get; set; }
}