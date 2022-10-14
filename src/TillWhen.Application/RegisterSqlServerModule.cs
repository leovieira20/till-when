using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TillWhen.Application.Projects;

namespace TillWhen.Application;

public static class RegisterApplicationModule
{
    public static void Register(IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddMediatR(typeof(CreateProject));
    }
}