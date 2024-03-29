using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TillWhen.Database.SqlServer.Repositories;
using TillWhen.Domain.Aggregates.QueueAggregate;
using TillWhen.Domain.Aggregates.WorkableAggregate;
using TillWhen.Domain.Aggregates.WorkableQueueConfigurationAggregate;

namespace TillWhen.Database.SqlServer;

public static class SqlServerModule
{
    public static WebApplicationBuilder AddSqlModule(this WebApplicationBuilder builder)
    {
        var services = builder.Services;
        
        var connectionString = builder.Configuration.GetConnectionString("TillWhen");

        services
            .AddDbContext<TillWhenContext>(x =>
            {
                x.UseSqlServer(connectionString);
            });

        services.AddScoped<IWorkableRepository, EfWorkableRepository>();
        services.AddScoped<IWorkableQueueRepository, EfWorkableQueueRepository>();
        services.AddScoped<IWorkableQueueConfigurationRepository, EfWorkableQueueConfigurationRepository>();

        return builder;
    }

    public static async Task MigrateDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<TillWhenContext>();
        await context.Database.MigrateAsync();
    }
}