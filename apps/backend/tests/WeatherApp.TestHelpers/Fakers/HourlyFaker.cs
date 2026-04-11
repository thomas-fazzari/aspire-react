using System.Globalization;
using Bogus;
using WeatherApp.Api.Features.Weather.GetHourly;

namespace WeatherApp.TestHelpers.Fakers;

internal static class HourlyFaker
{
    public static HourlyResponse Generate() =>
        new(
            Lat: new Faker().Address.Latitude(),
            Lon: new Faker().Address.Longitude(),
            Hours:
            [
                .. Enumerable
                    .Range(0, 24)
                    .Select(_ => new HourlyEntryDto(
                        Time: new Faker()
                            .Date.Soon(1)
                            .ToString("yyyy-MM-ddTHH:mm", CultureInfo.InvariantCulture),
                        Temperature: new Faker().Random.Double(-5, 35),
                        WeatherCode: new Faker().Random.Int(0, 99),
                        PrecipitationProbability: new Faker().Random.Double(0, 100),
                        WindSpeed: new Faker().Random.Double(0, 80)
                    )),
            ]
        );
}
