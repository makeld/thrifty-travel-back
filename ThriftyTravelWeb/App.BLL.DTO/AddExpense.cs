namespace App.BLL.DTO;

public class AddExpense
{
    public Guid TripId { get; set; }
    public string ExpenseTitle { get; set; } = default!;
    public string ExpenseType { get; set; } = default!;
    public string? ExpenseDescription { get; set; }
    public DateTime ExpenseDate { get; set; }
    public int ExpensePrice { get; set; }
    public string ExpenseCurrencyCode { get; set; } = default!;
    
    public string CountryName { get; set; } = default!;
    public string CountryContinent { get; set; } = default!;
    
    public string LocationName { get; set; } = default!;
    
    public string? ImageUrl { get; set; } = default!;
    
    public string? ImageDescription { get; set; }
}