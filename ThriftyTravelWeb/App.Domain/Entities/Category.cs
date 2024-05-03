using Domain.Base;

namespace Domain.Entities;

public class Category : BaseEntityId
{
    public string Name { get; set; } = default!;
    
    public ICollection<TripCategory>? TripCategories { get; set; } = new List<TripCategory>();
}