using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace TillWhen.Api.Infrastructure.Pipeline;

public static class OpenTelemetryRegistration
{
    public static WebApplicationBuilder AddOpenTelemetry(this WebApplicationBuilder builder)
    {
        var serviceName = builder.Configuration.GetValue<string>("ServiceName");
        var serviceVersion = builder.Configuration.GetValue<string>("ServiceVersion");
        
        builder.Services.AddOpenTelemetryTracing(tracerProviderBuilder =>
        {
            tracerProviderBuilder
                .AddConsoleExporter()
                .AddOtlpExporter()
                .AddSource(serviceName)
                .SetResourceBuilder(
                    ResourceBuilder.CreateDefault()
                        .AddService(
                            serviceName: serviceName,
                            serviceVersion: serviceVersion
                        )
                )
                .AddHttpClientInstrumentation()
                .AddAspNetCoreInstrumentation()
                .AddEntityFrameworkCoreInstrumentation()
                .AddSqlClientInstrumentation();
        });

        builder.Services.AddSingleton(TracerProvider.Default.GetTracer(serviceName));

        return builder;
    }
}