using Base.Contracts.Domain;

namespace App.DAL.DTO;

public class Location: IDomainEntityId
{
    public Guid Id { get; set; }
    public Guid CountryId { get; set; }
    public Country? Country { get; set; }

    public string LocationName { get; set; } = default!;
    public ICollection<TripLocation>? TripLocations { get; set; }

}