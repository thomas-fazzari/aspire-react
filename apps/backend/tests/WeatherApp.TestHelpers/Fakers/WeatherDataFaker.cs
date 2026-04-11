using Bogus;
using WeatherApp.Api.Domain.Weather;

namespace WeatherApp.TestHelpers.Fakers;

internal static class WeatherDataFaker
{
    public static WeatherData Generate() =>
        new Faker<WeatherData>()
            .CustomInstantiator(f => new WeatherData(
                Latitude: f.Address.Latitude(),
                Longitude: f.Address.Longitude(),
                Temperature: f.Random.Double(-30, 45),
                WindSpeed: f.Random.Double(0, 120),
                WeatherCode: f.Random.Int(0, 99)
            ))
            .Generate();
}
