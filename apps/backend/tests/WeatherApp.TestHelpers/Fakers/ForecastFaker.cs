using System.Globalization;
using Bogus;
using WeatherApp.Api.Features.Weather.GetForecast;

namespace WeatherApp.TestHelpers.Fakers;

internal static class ForecastFaker
{
    public static ForecastResponse Generate() =>
        new(
            Lat: new Faker().Address.Latitude(),
            Lon: new Faker().Address.Longitude(),
            Days:
            [
                .. Enumerable
                    .Range(0, 7)
                    .Select(_ => new DailyForecastDto(
                        Date: DateOnly
                            .FromDateTime(new Faker().Date.Soon(7))
                            .ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                        WeatherCode: new Faker().Random.Int(0, 99),
                        TemperatureMax: new Faker().Random.Double(10, 35),
                        TemperatureMin: new Faker().Random.Double(-5, 15),
                        PrecipitationProbability: new Faker().Random.Double(0, 100),
                        WindSpeedMax: new Faker().Random.Double(0, 80),
                        UvIndex: new Faker().Random.Double(0, 11),
                        Sunrise: "06:00",
                        Sunset: "20:00"
                    )),
            ]
        );
}
