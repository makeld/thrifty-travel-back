namespace App.DTO.v1_0;

public class Country
{
    public Guid Id { get; set; }

    public enum Name;
    public enum Continent;
    
    public ICollection<Location>? Locations { get; set; }

}