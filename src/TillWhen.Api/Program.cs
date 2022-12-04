using FastEndpoints;
using Serilog;
using TillWhen.Api.Infrastructure.Pipeline;
using TillWhen.Application;
using TillWhen.Database.SqlServer;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

try
{
    Log.Information("Starting web app");
    
    var builder = WebApplication.CreateBuilder(args);
    
    builder
        .AddSerilog()
        .AddSqlModule(builder.Configuration)
        .AddApplicationServices()
        .AddControllers()
        .AddSwagger();

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        await app.MigrateDatabaseAsync();
    }

    app.UseFastEndpoints();

    app.Run();

    Log.Information("Stopped cleanly");
    
    return 0;
}
catch (Exception e)
{
    Log.Fatal(e, "An unhandled exception occured during bootstrapping");
    return 1;
}
finally
{
    Log.CloseAndFlush();
}