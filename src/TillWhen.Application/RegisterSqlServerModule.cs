using MediatR;
using Microsoft.AspNetCore.Builder;
using TillWhen.Application.Workables;

namespace TillWhen.Application;

public static class RegisterApplicationModule
{
    public static WebApplicationBuilder AddApplicationServices(this WebApplicationBuilder builder)
    {
        var services = builder.Services;
        
        services
            .AddMediatR(typeof(CreateWorkable));

        return builder;
    }
}