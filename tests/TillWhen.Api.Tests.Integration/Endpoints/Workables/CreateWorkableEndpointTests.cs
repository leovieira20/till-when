using System.Net;
using System.Net.Http.Json;
using System.Text.Json.Nodes;
using FluentAssertions;

namespace TillWhen.Api.Tests.Integration.Endpoints.Workables;

public class CreateWorkableEndpointTests : IClassFixture<TillWhenApi>
{
    private readonly HttpClient _client;

    public CreateWorkableEndpointTests(TillWhenApi api)
    {
        _client = api.CreateClient();
    }
    
    [Fact]
    public async Task CanCreateProject()
    {
        var title = Guid.NewGuid().ToString();
        var response = await _client.PostAsJsonAsync("api/workable", new
        {
            Title = title,
            Duration = "1d 2h 3m"
        });
        
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        
        var createdProject = await _client.GetFromJsonAsync<JsonObject>(response.Headers.Location);
        
        var project = createdProject!["Workable"]!.AsObject();

        project["id"].Should().NotBeNull();
        project["Title"]!.GetValue<string>().Should().Be(title);
        
        var jsonDuration = project["Duration"] as JsonObject;
        jsonDuration!["Days"]!.GetValue<int>().Should().Be(1);
        jsonDuration["Hours"]!.GetValue<int>().Should().Be(2);
        jsonDuration["Minutes"]!.GetValue<int>().Should().Be(3);
    }
}