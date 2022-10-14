using FastEndpoints;

namespace TillWhen.Api.Infrastructure.Pipeline;

public static class ControllersRegistration
{
    public static WebApplicationBuilder AddControllers(this WebApplicationBuilder builder)
    {
        // builder.Services.AddControllers();
        builder.Services.AddFastEndpoints();
        
        return builder;
    }
}