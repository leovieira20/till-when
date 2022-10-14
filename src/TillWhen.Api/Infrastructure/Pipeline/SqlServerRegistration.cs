using TillWhen.Database.SqlServer;

namespace TillWhen.Api.Infrastructure.Pipeline;

public static class SqlServerRegistration
{
    public static WebApplicationBuilder AddSqlServerDatabase(this WebApplicationBuilder builder)
    {
        RegisterSqlServerModule.Register(builder.Services, builder.Configuration);
        
        return builder;
    }
}