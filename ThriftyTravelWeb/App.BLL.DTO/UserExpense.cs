using Base.Contracts.Domain;

namespace App.BLL.DTO;

public class UserExpense: IDomainEntityId
{
    public Guid Id { get; set; }
    public Guid ExpenseId { get; set; }
    public Guid TripUserId { get; set; }
}