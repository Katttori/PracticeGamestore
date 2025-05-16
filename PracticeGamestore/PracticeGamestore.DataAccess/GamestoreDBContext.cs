using System.Reflection;
using Microsoft.EntityFrameworkCore;
using PracticeGamestore.DataAccess.Entities;
using File = PracticeGamestore.DataAccess.Entities.File;

namespace PracticeGamestore.DataAccess;

public class GamestoreDbContext : DbContext
{
    public DbSet<Game> Games { get; set; }
    public DbSet<Country> Countries { get; set; }
    public DbSet<Blacklist> Blacklists { get; set; }
    public DbSet<Publisher> Publishers { get; set; }
    public DbSet<File> Files { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<Platform> Platforms { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<GameOrder> GameOrders { get; set; }
    public DbSet<GamePlatform> GamePlatforms { get; set; }
    public DbSet<GameGenre> GameGenres { get; set; }

    public GamestoreDbContext(DbContextOptions<GamestoreDbContext> options) : base(options){}
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}