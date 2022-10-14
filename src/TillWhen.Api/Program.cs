using TillWhen.Database.SqlServer;

var builder = WebApplication.CreateBuilder(args);

RegisterSqlServerModule.Register(builder.Services, builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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