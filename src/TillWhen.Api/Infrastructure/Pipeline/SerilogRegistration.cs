using Serilog;

namespace TillWhen.Api.Infrastructure.Pipeline;

public static class SerilogRegistration
{
    public static WebApplicationBuilder AddSerilog(this WebApplicationBuilder builder)
    {
        var seqServerUrl = builder.Configuration["SeqServerUrl"];

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .WriteTo.Console()
            .WriteTo.Seq(seqServerUrl)
            .Enrich.WithProperty("ApplicationName", "TillWhen.API")
            .CreateLogger();

        builder.Host.UseSerilog();

        return builder;
    }
}