using Base.Contracts.Domain;

namespace App.DAL.DTO;

public class Trip: IDomainEntityId
{
    public Guid Id { get; set; }
    public string Title { get; set; } = default!;
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    // public ICollection<Expense>? Expenses { get; set; }
    // public ICollection<Photo>? Photos { get; set; }
    // public ICollection<Comment>? Comments { get; set; }
    // public ICollection<Like>? Likes { get; set; }
    // public ICollection<TripCategory>? TripCategories { get; set; }
    // public ICollection<TripLocation>? TripLocations { get; set; }
    // public ICollection<TripUser>? TripUsers { get; set; }
}
