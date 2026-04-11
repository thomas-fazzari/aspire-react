using System.Text.Json.Serialization;

namespace WeatherApp.Api.Infrastructure.OpenMeteo;

internal sealed record OpenMeteoResponse(
    [property: JsonPropertyName("latitude")] double Latitude,
    [property: JsonPropertyName("longitude")] double Longitude,
    [property: JsonPropertyName("current")] OpenMeteoCurrentConditions Current
);

internal sealed record OpenMeteoCurrentConditions(
    [property: JsonPropertyName("temperature_2m")] double Temperature,
    [property: JsonPropertyName("wind_speed_10m")] double WindSpeed,
    [property: JsonPropertyName("weather_code")] int WeatherCode
);
