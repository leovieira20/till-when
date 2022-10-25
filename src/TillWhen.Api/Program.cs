using FastEndpoints;
using TillWhen.Api.Infrastructure.Pipeline;
using TillWhen.Database.SqlServer;

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
app.UseWelcomePage();
app.Run();