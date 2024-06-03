using Base.Contracts.Domain;
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

    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // disable cascade delete initially for everything
        foreach (var relationship in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
        {
            relationship.DeleteBehavior = DeleteBehavior.Restrict;
        }
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        ConvertDateTimesToUtc();
        UpdateMetaInfo();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void ConvertDateTimesToUtc()
    {
        foreach (var entity in ChangeTracker.Entries().Where(e => e.State != EntityState.Deleted))
        {
            foreach (var prop in entity
                         .Properties
                         .Where(x => x.Metadata.ClrType == typeof(DateTime) && x.CurrentValue != null)
                    )
            {
                prop.CurrentValue = ((DateTime) prop.CurrentValue!).ToUniversalTime();
            }
        }
    }

    private void UpdateMetaInfo()
    {
        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry.Entity is IDomainEntityMetadata metaDataEntity)
            {
                
                switch (entry.State)
                {
                    case EntityState.Added:
                        metaDataEntity.CreatedAt = DateTime.Now.ToUniversalTime();
                        metaDataEntity.UpdatedAt = DateTime.Now.ToUniversalTime();
                        break;
                    case EntityState.Modified:
                        metaDataEntity.UpdatedAt = DateTime.Now.ToUniversalTime();
                        entry.Property("CreatedAt").IsModified = false;
                        break;
                }
            }
        }
    }


}