using System.ComponentModel.DataAnnotations;

namespace App.DTO.v1_0;

public class Photo
{
    public Guid Id { get; set; }
    public Guid? TripId { get; set; }
    public Trip? Trip { get; set; }

    public Guid? ExpenseId { get; set; }
    public Expense? Expense { get; set; }
    
    [MaxLength(256)]
    public string ImageUrl { get; set; } = default!;
    
    [MaxLength(128)]
    public string? Description { get; set; }
}