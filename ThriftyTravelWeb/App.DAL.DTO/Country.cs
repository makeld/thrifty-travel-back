using Base.Contracts.Domain;

namespace App.DAL.DTO;

public class Country: IDomainEntityId
{
    public Guid Id { get; set; }

    public enum Name;
    
    public enum Continent;
    
    public ICollection<Location>? Locations { get; set; }

}