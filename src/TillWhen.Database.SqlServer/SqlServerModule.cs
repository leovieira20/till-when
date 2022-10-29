using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TillWhen.Database.SqlServer.Repositories;
using TillWhen.Domain.Aggregates.ProjectAggregate;
using TillWhen.Domain.Aggregates.QueueAggregate;

namespace TillWhen.Database.SqlServer;

public static class SqlServerModule
{
    public static WebApplicationBuilder AddSqlModule(this WebApplicationBuilder builder, IConfiguration configuration)
    {
        var services = builder.Services;
        
        var connectionString = configuration.GetConnectionString("TillWhen");

        services
            .AddDbContext<TillWhenContext>(x =>
            {
                x.UseSqlServer(connectionString);
            });

        services.AddScoped<IProjectRepository, EfProjectRepository>();
        services.AddScoped<ITaskQueueRepository, EfTaskQueueRepository>();

        return builder;
    }

    public static async Task MigrateDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<TillWhenContext>();
        await context.Database.MigrateAsync();
    }
}