namespace App.DTO.v1_0;

public class Country
{
    public Guid Id { get; set; }

    public string Name { get; set; } = default!;
    
    public string Continent { get; set; } = default!;
    
}