using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TillWhen.Database.SqlServer;

public class RegisterSqlServerModule
{
    public static void Register(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("TillWhen");

        services
            .AddDbContext<TillWhenContext>(x =>
            {
                x.UseSqlServer(connectionString);
            });
    }
}