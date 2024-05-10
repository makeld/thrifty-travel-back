using Domain.Base;

namespace Domain.Entities;

public class Country : BaseEntityId
{
    public enum Name;
    public enum Continent;
    
    public ICollection<Location>? Locations { get; set; }
}