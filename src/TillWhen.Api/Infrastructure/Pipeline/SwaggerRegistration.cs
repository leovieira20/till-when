namespace TillWhen.Api.Infrastructure.Pipeline;

public static class SwaggerRegistration
{
    public static WebApplicationBuilder AddSwagger(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        
        return builder;
    }
}