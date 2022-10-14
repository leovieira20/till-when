using System.Net;
using System.Net.Http.Json;
using System.Text.Json.Nodes;
using FluentAssertions;

namespace TillWhen.Api.Tests.Integration.Endpoints.Projects;

public class CreateProjectEndpointTests : IClassFixture<TillWhenApi>
{
    private readonly HttpClient _client;

    public CreateProjectEndpointTests(TillWhenApi api)
    {
        _client = api.CreateClient();
    }
    
    [Fact]
    public async Task CanCreateProject()
    {
        var response = await _client.PostAsync("api/projects", null);
        
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        
        var location = response.Headers.Location;
        
        var createdProject = await _client.GetFromJsonAsync<JsonObject>(location);
        
        var project = createdProject!["project"]!;
        project.AsObject()["id"].Should().NotBeNull();
    }
}