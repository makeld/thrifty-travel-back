using Domain.Base;

namespace Domain.Entities;

public class Trip : BaseEntityId
{
    public string Title { get; set; } = default!;
    public string? Description { get; set; } = default!;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    
    public ICollection<Expense>? Expenses { get; set; } = new List<Expense>();
    public ICollection<Photo>? Photos { get; set; } = new List<Photo>();
    public ICollection<Comment>? Comments { get; set; } = new List<Comment>();
    public ICollection<Like>? Likes { get; set; } = new List<Like>();
    public ICollection<TripCategory>? TripCategories { get; set; } = new List<TripCategory>();
    public ICollection<TripLocation>? TripLocations { get; set; } = new List<TripLocation>();
    public ICollection<TripUser>? TripUsers { get; set; } = new List<TripUser>();
}