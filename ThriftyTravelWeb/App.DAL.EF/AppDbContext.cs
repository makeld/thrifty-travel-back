using Domain.Identity;
using Domain.Entities;
using Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF;

public class AppDbContext : IdentityDbContext<AppUser, AppRole, Guid, IdentityUserClaim<Guid>, AppUserRole, 
    IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>
{
    public DbSet<AppUser> AppUsers { get; set; } = default!;
    public DbSet <Category> Categories { get; set; } = default!;
    public DbSet<Comment> Comments { get; set; } = default!;
    public DbSet<Country> Countries { get; set; } = default!;
    public DbSet<Expense> Expenses { get; set; } = default!;
    public DbSet<Like> Likes { get; set; } = default!;
    public DbSet<Location> Locations { get; set; } = default!;
    public DbSet<Photo> Photos { get; set; } = default!;
    public DbSet<Trip> Trips { get; set; } = default!;
    public DbSet<TripCategory> TripCategories { get; set; } = default!;
    public DbSet<TripLocation> TripLocations { get; set; } = default!;
    public DbSet<TripUser> TripUsers { get; set; } = default!;
    public DbSet<UserExpense> UserExpenses { get; set; } = default!;
    
    
    public DbSet<AppRefreshToken> RefreshTokens { get; set; } = default!;
    
    public AppDbContext(DbContextOptions options)
        : base(options)
    {
    }
    
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        foreach (var entity in ChangeTracker.Entries().Where(e => e.State != EntityState.Deleted))
        {
            foreach (var prop in entity
                         .Properties
                         .Where(x => x.Metadata.ClrType == typeof(DateTime)))
            {
                Console.WriteLine(prop);
                prop.CurrentValue = ((DateTime) prop.CurrentValue).ToUniversalTime();
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }


}