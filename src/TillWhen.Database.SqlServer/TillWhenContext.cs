using Microsoft.EntityFrameworkCore;
using TillWhen.Domain.Aggregates.QueueAggregate;
using TillWhen.Domain.Aggregates.WorkableAggregate;
using TillWhen.Domain.Common;

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
            .Entity<WorkableQueue>()
            .Ignore(x => x.Capacity)
            .Ignore(x => x.Workables);
        
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
            .Ignore(x => x.RemainingEffort);
        
        modelBuilder
            .Entity<Workable>()
            .OwnsOne(x => x.Estimation, builder =>
            {
                builder.Ignore(x => x.Days);
                builder.Ignore(x => x.Hours);
                builder.Ignore(x => x.TotalHours);
                builder.Ignore(x => x.Minutes);
                builder.Ignore(x => x.Tomatoes);
            });
    }

    public DbSet<Workable> Workables { get; set; }
    public DbSet<WorkableQueue> WorkableQueues { get; set; }
}