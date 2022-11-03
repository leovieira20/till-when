using Microsoft.EntityFrameworkCore;
using TillWhen.Domain.Aggregates.ProjectAggregate;
using TillWhen.Domain.Aggregates.QueueAggregate;

namespace TillWhen.Database.SqlServer;

public class TillWhenContext : DbContext
{
    protected TillWhenContext()
    {
    }

    public TillWhenContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<Project>()
            .Property(x => x.Title)
            .HasMaxLength(200);
        
        modelBuilder
            .Entity<Project>()
            .Property(x => x.Category)
            .HasMaxLength(100);
        
        modelBuilder
            .Entity<Project>()
            .OwnsOne(x => x.Duration);
    }

    public DbSet<Project> Projects { get; set; }
    public DbSet<TaskQueue> TaskQueues { get; set; }
}