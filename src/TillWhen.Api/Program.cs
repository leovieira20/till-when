using TillWhen.Api.Infrastructure.Pipeline;

var builder = WebApplication.CreateBuilder(args);

builder
    .AddSerilog()
    .AddEf()
    .AddControllers()
    .AddSwagger();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();
app.UseWelcomePage();
app.Run();