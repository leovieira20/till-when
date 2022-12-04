using Serilog;

namespace TillWhen.Api.Infrastructure.Pipeline;

public static class SerilogRegistration
{
    public static WebApplicationBuilder AddSerilog(this WebApplicationBuilder builder)
    {
        builder
            .Host
            .UseSerilog((context, _, configuration) =>
                {
                    configuration.ReadFrom.Configuration(context.Configuration);
                }
            );

        return builder;
    }
}