using Bogus;
using WeatherApp.Api.Features.Weather.GetCurrentWeather;

namespace WeatherApp.TestHelpers.Fakers;

internal static class WeatherConditionsFaker
{
    public static WeatherConditionsDto Generate() =>
        new(
            Lat: new Faker().Address.Latitude(),
            Lon: new Faker().Address.Longitude(),
            Temperature: new Faker().Random.Double(-30, 45),
            WindSpeed: new Faker().Random.Double(0, 120),
            WeatherCode: new Faker().Random.Int(0, 99),
            RelativeHumidity: new Faker().Random.Double(0, 100),
            ApparentTemperature: new Faker().Random.Double(-40, 50),
            WindDirection: new Faker().Random.Double(0, 360),
            SurfacePressure: new Faker().Random.Double(950, 1050),
            IsDay: new Faker().Random.Bool()
        );
}
