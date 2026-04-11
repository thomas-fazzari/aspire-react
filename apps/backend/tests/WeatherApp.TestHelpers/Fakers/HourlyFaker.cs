using System.Globalization;
using Bogus;
using WeatherApp.Api.Features.Weather.GetHourly;
using WeatherApp.Api.Shared.Constants;

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
                    .Select(_ => new HourlyConditionsDto(
                        Time: new Faker()
                            .Date.Soon(1)
                            .ToString(Formats.DateTime, CultureInfo.InvariantCulture),
                        Temperature: new Faker().Random.Double(-5, 35),
                        WeatherCode: new Faker().Random.Int(0, 99),
                        PrecipitationProbability: new Faker().Random.Double(0, 100),
                        WindSpeed: new Faker().Random.Double(0, 80)
                    )),
            ]
        );
}
