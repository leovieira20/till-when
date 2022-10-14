using TillWhen.Database.SqlServer;

namespace TillWhen.Api.Infrastructure.Pipeline;

public static class EfRegistration
{
    public static WebApplicationBuilder AddEf(this WebApplicationBuilder builder)
    {
        RegisterSqlServerModule.Register(builder.Services, builder.Configuration);
        
        return builder;
    }
}