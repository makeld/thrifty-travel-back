
namespace Domain.Entities;

public class UserExpense : BaseEntity
{
    public Guid ExpenseId { get; set; }
    public Expense? Expense { get; set; }
    
    public Guid UserId { get; set; }
    public TripUser? TripUser { get; set; }
}