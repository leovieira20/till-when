using Microsoft.EntityFrameworkCore;
using TillWhen.Domain.Aggregates.ProjectAggregate;
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
            .Entity<Project>()
            .OwnsOne(x => x.Duration);
    }

    public DbSet<Project> Projects { get; set; }
}