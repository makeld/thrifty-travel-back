namespace Domain.Entities;

public class Category : BaseEntity
{
    public enum Name;

    public ICollection<TripCategory>? TripCategories { get; set; } = new List<TripCategory>();
}