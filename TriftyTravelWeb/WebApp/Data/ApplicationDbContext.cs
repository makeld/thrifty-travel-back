using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public DbSet<Category> Categories { get; set; } = default!;
    public DbSet<AppUser> AppUsers { get; set; } = default!;
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
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
}