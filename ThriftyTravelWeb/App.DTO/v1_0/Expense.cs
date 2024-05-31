using System.ComponentModel.DataAnnotations;

namespace App.DTO.v1_0;

public class Expense
{
    public Guid Id { get; set; }
    public Guid TripId { get; set; }
    
    [MaxLength(128)]
    public string Title { get; set; } = default!;
    
    [MaxLength(128)]
    public string Type { get; set; } = default!;
    
    [MaxLength(512)]
    public string? Description { get; set; }
    
    public DateTime ExpenseDate { get; set; }
    
    public int ExpensePrice { get; set; }
    
    [MaxLength(3)]
    public string CurrencyCode { get; set; } = default!;

}