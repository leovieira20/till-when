using TillWhen.Database.SqlServer;

namespace TillWhen.Api.Infrastructure.Pipeline;

public static class SqlServerRegistration
{
    public static WebApplicationBuilder AddSqlServerDatabase(this WebApplicationBuilder builder)
    {
        SqlServerModule.Register(builder.Services, builder.Configuration);
        
        return builder;
    }
}