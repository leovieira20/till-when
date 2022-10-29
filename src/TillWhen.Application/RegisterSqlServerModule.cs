using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TillWhen.Application.Projects;
using TillWhen.Domain.Aggregates.QueueAggregate;

namespace TillWhen.Application;

public static class RegisterApplicationModule
{
    public static WebApplicationBuilder AddApplicationServices(this WebApplicationBuilder builder)
    {
        var services = builder.Services;
        
        services
            .AddMediatR(typeof(CreateProject));

        return builder;
    }
}