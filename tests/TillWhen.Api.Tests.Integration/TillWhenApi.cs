using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TillWhen.Database.SqlServer;

namespace TillWhen.Api.Tests.Integration;

public class TillWhenApi : WebApplicationFactory<IApiMarker>, IAsyncLifetime
{
    private readonly string _sqlServerPassword = Guid.NewGuid().ToString();
    
    private readonly TestcontainersContainer _sqlServerContainerBuilder;

    public TillWhenApi()
    {
        _sqlServerContainerBuilder = new TestcontainersBuilder<TestcontainersContainer>()
            .WithImage("mcr.microsoft.com/azure-sql-edge")
            .WithEnvironment("ACCEPT_EULA", "1")
            .WithEnvironment("MSSQL_SA_PASSWORD", _sqlServerPassword)
            .WithEnvironment("MSSQL_PID", "Developer")
            .WithEnvironment("MSSQL_USER", "sa")
            .WithPortBinding(1433, true)
            .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(1433))
            .Build();
    }
    
    public async Task InitializeAsync()
    {
        await _sqlServerContainerBuilder
            .StartAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var connectionString = $"Server={_sqlServerContainerBuilder.Hostname},{_sqlServerContainerBuilder.GetMappedPublicPort(1433)};Database=TillWhen;User Id=sa;Password={_sqlServerPassword};MultipleActiveResultSets=true;TrustServerCertificate=true;";

        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll(typeof(DbContextOptions<TillWhenContext>));
            services
                .AddDbContext<TillWhenContext>(x =>
                {
                    x.UseSqlServer(connectionString);
                });
        });
    }

    public new async Task DisposeAsync()
    {
        await _sqlServerContainerBuilder
            .StopAsync();
    }
}