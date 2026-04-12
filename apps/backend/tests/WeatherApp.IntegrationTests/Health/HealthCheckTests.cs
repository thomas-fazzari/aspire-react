using System.Net;
using WeatherApp.IntegrationTests.Fixtures;

namespace WeatherApp.IntegrationTests.Health;

public sealed class HealthCheckTests(ApiFixture fixture)
{
    [Fact]
    public async Task AliveEndpoint_ReturnsOk()
    {
        using var client = fixture.CreateApiClient();
        var ct = TestContext.Current.CancellationToken;

        var response = await client.GetAsync("/alive", ct);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task HealthEndpoint_ReturnsOk()
    {
        using var client = fixture.CreateApiClient();
        var ct = TestContext.Current.CancellationToken;

        var response = await client.GetAsync("/health", ct);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
