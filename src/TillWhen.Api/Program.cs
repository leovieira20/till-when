using FastEndpoints;
using TillWhen.Api.Infrastructure.Pipeline;

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
}

app.UseFastEndpoints();
app.UseWelcomePage();
app.Run();