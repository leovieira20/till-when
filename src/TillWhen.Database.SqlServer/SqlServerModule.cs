using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TillWhen.Database.SqlServer.Repositories;
using TillWhen.Domain.Aggregates.ProjectAggregate;

namespace TillWhen.Database.SqlServer;

public static class SqlServerModule
{
    public static void Register(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("TillWhen");

        services
            .AddDbContext<TillWhenContext>(x =>
            {
                x.UseSqlServer(connectionString);
            });

        services.AddScoped<IProjectRepository, EfProjectRepository>();
    }

    public static async Task MigrateDatabaseAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<TillWhenContext>();
        await context.Database.MigrateAsync();
    }
}