using System.Net;
using System.Net.Http.Json;
using WeatherApp.IntegrationTests.Fixtures;

namespace WeatherApp.IntegrationTests.Cities.GetCities;

public sealed class EndpointTests(ApiFixture fixture)
{
    [Fact]
    public async Task GetCities_ReturnsOkWithContent()
    {
        using var client = fixture.CreateApiClient();
        var ct = TestContext.Current.CancellationToken;

        var response = await client.GetAsync("/cities", ct);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var cities = await response.Content.ReadFromJsonAsync<JsonElement[]>(ct);
        Assert.NotNull(cities);
        Assert.Equal(5, cities.Length);
    }
}
