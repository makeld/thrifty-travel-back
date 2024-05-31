using Domain.Base;

namespace Domain.Entities;

public class Trip : BaseEntityIdMetadata
{
    public string Title { get; set; } = default!;
    public string? Description { get; set; } = default!;
    
    public ICollection<Expense>? Expenses { get; set; }
    public ICollection<Photo>? Photos { get; set; }
    public ICollection<Comment>? Comments { get; set; }
    public ICollection<Like>? Likes { get; set; }
    public ICollection<TripCategory>? TripCategories { get; set; }
    public ICollection<TripLocation>? TripLocations { get; set; }
    public ICollection<TripUser>? TripUsers { get; set; }
}