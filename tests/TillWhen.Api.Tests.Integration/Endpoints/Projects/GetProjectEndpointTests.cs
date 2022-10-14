using System.Net;
using FluentAssertions;

namespace TillWhen.Api.Tests.Integration.Endpoints.Projects;

public class GetProjectEndpointTests : IClassFixture<TillWhenApi>
{
    private readonly HttpClient _client;

    public GetProjectEndpointTests(TillWhenApi api)
    {
        _client = api.CreateClient();
    }

    [Fact]
    public async Task ReturnsNoContentWhenProjectDoesNotExist()
    {
        var response = await _client.GetAsync($"api/projects/{Guid.NewGuid()}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}