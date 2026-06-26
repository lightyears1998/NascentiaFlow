using Microsoft.EntityFrameworkCore;

namespace NascentiaFlow.Entities;

public class CoreContext(AppEnvironment appEnvironment) : DbContext
{
    public DbSet<Focus> Focuses { get; set; }
    public DbSet<Diary> Diaries { get; set; }
    public DbSet<Source> Sources { get; set; }
    public DbSet<Activity> Activities { get; set; }
    public DbSet<ActivityType> ActivityTypes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite($"Data Source={appEnvironment.CoreDbPath}", builder =>
        {
            builder.UseNodaTime();
        });
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }
}

public class EditionContext(AppEnvironment appEnvironment) : DbContext
{
    public DbSet<Edition> Editions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite($"Data Source={appEnvironment.EditionDbPath}", builder =>
        {
            builder.UseNodaTime();
        });
    }
}
