using System.ComponentModel.DataAnnotations;

namespace App.DTO.v1_0;

public class AddExpense
{
    public Guid ExpenseId { get; set; }
    public Guid TripId { get; set; }
    public Guid TripLocationId { get; set; }
    
    [MaxLength(128)]
    public string ExpenseTitle { get; set; } = default!;
    
    [MaxLength(128)]
    public string ExpenseType { get; set; } = default!;
    
    [MaxLength(512)]
    public string? ExpenseDescription { get; set; }
    
    public DateTime ExpenseDate { get; set; }
    
    public int ExpensePrice { get; set; }
    
    [MaxLength(3)]
    public string ExpenseCurrencyCode { get; set; } = default!;

    [MaxLength(128)]
    public string CountryName { get; set; } = default!;
    
    [MaxLength(128)]
    public string CountryContinent { get; set; } = default!;
    
    [MaxLength(128)]
    public string LocationName { get; set; } = default!;
    
    [MaxLength(256)]
    public string? ImageUrl { get; set; } = default!;
    
    [MaxLength(128)]
    public string? ImageDescription { get; set; }
}