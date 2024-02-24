using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

public class AppUser : IdentityUser<Guid>
{
    public ICollection<Like>? Likes { get; set; } = new List<Like>();
    public ICollection<Comment>? Comments { get; set; } = new List<Comment>();
    public ICollection<TripUser>? TripUsers { get; set; } = new List<TripUser>();
}