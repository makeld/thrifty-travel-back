namespace Domain.Entities;

public class Country : BaseEntity
{
    public enum Name;
    public enum Continent;
    
    public ICollection<Location>? Locations { get; set; } = new List<Location>();
}