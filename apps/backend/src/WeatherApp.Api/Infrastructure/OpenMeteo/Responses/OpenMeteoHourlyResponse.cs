using System.Text.Json.Serialization;

namespace WeatherApp.Api.Infrastructure.OpenMeteo.Responses;

internal sealed record OpenMeteoHourlyResponse(
    [property: JsonPropertyName("latitude")] double Latitude,
    [property: JsonPropertyName("longitude")] double Longitude,
    [property: JsonPropertyName("hourly")] OpenMeteoHourly Hourly
);

internal sealed record OpenMeteoHourly(
    [property: JsonPropertyName("time")] string[] Time,
    [property: JsonPropertyName("temperature_2m")] double[] Temperature,
    [property: JsonPropertyName("weather_code")] int[] WeatherCode,
    [property: JsonPropertyName("precipitation_probability")] double[] PrecipitationProbability,
    [property: JsonPropertyName("wind_speed_10m")] double[] WindSpeed
);
