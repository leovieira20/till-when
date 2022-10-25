using FastEndpoints;
using TillWhen.Api.Infrastructure.Pipeline;
using TillWhen.Database.SqlServer;

var builder = WebApplication.CreateBuilder(args);

builder
    .AddSerilog()
    .AddSqlServerDatabase()
    .AddApplicationServices()
    .AddControllers()
    .AddSwagger();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    await SqlServerModule.MigrateDatabaseAsync(app.Services);
}

app.UseFastEndpoints();
app.UseWelcomePage();
app.Run();