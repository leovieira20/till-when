using TillWhen.Application;

namespace TillWhen.Api.Infrastructure.Pipeline;

public static class ApplicationRegistration
{
    public static WebApplicationBuilder AddApplicationServices(this WebApplicationBuilder builder)
    {
        RegisterApplicationModule.Register(builder.Services, builder.Configuration);
        
        return builder;
    }
}