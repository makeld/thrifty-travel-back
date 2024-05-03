using Domain.Base;

namespace Domain.Entities;

public class Location : BaseEntityId
{
    public Guid CountryId { get; set; }
    public Country? Country { get; set; }

    public string LocationName { get; set; } = default!;

    public ICollection<TripLocation>? TripLocations { get; set; } = new List<TripLocation>();
}