using Domain.Base;

namespace Domain.Entities;

public class UserExpense : BaseEntityId
{
    public Guid ExpenseId { get; set; }
    public Expense? Expense { get; set; }
    
    public Guid TripUserId { get; set; }
    public TripUser? TripUser { get; set; }
}