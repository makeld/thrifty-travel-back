namespace App.DTO.v1_0;

public class UserExpense
{
    public Guid Id { get; set; }
    public Guid ExpenseId { get; set; }
    public Guid TripUserId { get; set; }
}