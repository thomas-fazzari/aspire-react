using System.Text.Json.Serialization;

namespace WeatherApp.Api.Infrastructure.OpenMeteo.Responses;

internal sealed record OpenMeteoForecastResponse(
    [property: JsonPropertyName("latitude")] double Latitude,
    [property: JsonPropertyName("longitude")] double Longitude,
    [property: JsonPropertyName("daily")] OpenMeteoDaily Daily
);

internal sealed record OpenMeteoDaily(
    [property: JsonPropertyName("time")] string[] Time,
    [property: JsonPropertyName("weather_code")] int[] WeatherCode,
    [property: JsonPropertyName("temperature_2m_max")] double[] TemperatureMax,
    [property: JsonPropertyName("temperature_2m_min")] double[] TemperatureMin,
    [property: JsonPropertyName("precipitation_probability_max")]
        double[] PrecipitationProbabilityMax,
    [property: JsonPropertyName("wind_speed_10m_max")] double[] WindSpeedMax,
    [property: JsonPropertyName("uv_index_max")] double[] UvIndexMax,
    [property: JsonPropertyName("sunrise")] string[] Sunrise,
    [property: JsonPropertyName("sunset")] string[] Sunset
);
