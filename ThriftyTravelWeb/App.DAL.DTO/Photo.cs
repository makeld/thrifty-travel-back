using Base.Contracts.Domain;

namespace App.DAL.DTO;

public class Photo: IDomainEntityId
{
    public Guid Id { get; set; }
    public Guid? TripId { get; set; }
    public Trip? Trip { get; set; }

    public Guid? ExpenseId { get; set; }
    public Expense? Expense { get; set; }

    public string ImageUrl { get; set; } = default!;
    public string? Description { get; set; }
}