using WeatherApp.IntegrationTests.Fixtures;

namespace WeatherApp.IntegrationTests.Weather;

public abstract class WeatherIntegrationTests(ApiFixture fixture)
{
    protected ApiFixture Fixture { get; } = fixture;
}
