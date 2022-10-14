using Microsoft.EntityFrameworkCore;
using TillWhen.Domain.Entities;

namespace TillWhen.Database.SqlServer;

public class TillWhenContext : DbContext
{
    protected TillWhenContext()
    {
    }

    public TillWhenContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Project> Projects { get; set; }
}