using Microsoft.EntityFrameworkCore;
using TillWhen.Domain.Aggregates.QueueAggregate;
using TillWhen.Domain.Aggregates.WorkableAggregate;

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
            .Entity<Workable>()
            .Property(x => x.Title)
            .HasMaxLength(200);
        
        modelBuilder
            .Entity<Workable>()
            .Property(x => x.Category)
            .HasMaxLength(100);
        
        modelBuilder
            .Entity<Workable>()
            .OwnsOne(x => x.Duration);
    }

    public DbSet<Workable> Workables { get; set; }
    public DbSet<WorkableQueue> WorkableQueues { get; set; }
}