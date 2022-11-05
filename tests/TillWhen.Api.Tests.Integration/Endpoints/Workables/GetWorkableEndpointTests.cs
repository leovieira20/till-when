using System.Net;
using FluentAssertions;

namespace TillWhen.Api.Tests.Integration.Endpoints.Workables;

public class GetWorkableEndpointTests : IClassFixture<TillWhenApi>
{
    private readonly HttpClient _client;

    public GetWorkableEndpointTests(TillWhenApi api)
    {
        _client = api.CreateClient();
    }

    [Fact]
    public async Task ReturnsNoContentWhenProjectDoesNotExist()
    {
        var response = await _client.GetAsync($"api/workable/{Guid.NewGuid()}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}