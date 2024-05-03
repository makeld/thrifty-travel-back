using Base.Domain;
using Domain.Identity;

namespace App.Domain.Identity;

public class AppRefreshToken : BaseRefreshToken
{
    public Guid AppUserId { get; set; }
    public AppUser? AppUser { get; set; }
}