using Microsoft.EntityFrameworkCore;
using TillWhen.Domain.Aggregates.QueueAggregate;
using TillWhen.Domain.Aggregates.WorkableAggregate;
using TillWhen.Domain.Aggregates.WorkableQueueConfigurationAggregate;
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
            .Ignore(x => x.Capacity);

        modelBuilder
            .Entity<WorkableQueue>()
            .HasOne<WorkableQueueConfiguration>()
            .WithOne();
        
        modelBuilder
            .Entity<WorkableQueueConfiguration>()
            .OwnsOne(x => x.Capacity, builder =>
            {
                builder.Ignore(x => x.Days);
                builder.Ignore(x => x.Hours);
                builder.Ignore(x => x.TotalHours);
                builder.Ignore(x => x.Minutes);
                builder.Ignore(x => x.Tomatoes);

                builder
                    .Property(x => x.OriginalDuration)
                    .HasMaxLength(15);

                builder
                    .Property(x => x.OriginalDuration)
                    .UsePropertyAccessMode(PropertyAccessMode.Property);
            });

        modelBuilder
            .Entity<WorkableBase>()
            .Property(x => x.Title)
            .HasMaxLength(200);
        
        modelBuilder
            .Entity<WorkableBase>()
            .Property(x => x.Category)
            .HasMaxLength(100);

        modelBuilder
            .Entity<WorkableBase>()
            .OwnsOne(x => x.Estimation, builder =>
            {
                builder.Ignore(x => x.Days);
                builder.Ignore(x => x.Hours);
                builder.Ignore(x => x.TotalHours);
                builder.Ignore(x => x.Minutes);
                builder.Ignore(x => x.Tomatoes);

                builder
                    .Property(x => x.OriginalDuration)
                    .HasMaxLength(15);

                builder
                    .Property(x => x.OriginalDuration)
                    .UsePropertyAccessMode(PropertyAccessMode.Property);
            });
        
        modelBuilder.Entity<Workable>()
            .HasDiscriminator<string>("workable_type")
            .HasValue<Workable>("workable_base");
        
        modelBuilder
            .Entity<Workable>()
            .Property("workable_type")
            .HasMaxLength(20);
    }

    public DbSet<Workable> Workables { get; set; }
    public DbSet<WorkableQueue> WorkableQueues { get; set; }
    public DbSet<WorkableQueueConfiguration> WorkableQueueConfigurations { get; set; }
}