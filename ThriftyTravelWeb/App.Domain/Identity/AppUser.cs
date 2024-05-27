using System.ComponentModel.DataAnnotations;
using Domain.Identity;
using Base.Contracts.Domain;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Domain.Identity;

public class AppUser : IdentityUser<Guid>, IDomainEntityId
{ 
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    
    public ICollection<TripUser>? TripUsers { get; set; }
    public ICollection<Like>? Likes { get; set; }
    public ICollection<Comment>? Comments { get; set; }

    public ICollection<AppRefreshToken>? RefreshTokens { get; set; }
}