using System.Text.Json.Serialization;

namespace WeatherApp.Api.Infrastructure.OpenMeteo.Responses;

internal sealed record OpenMeteoResponse(
    [property: JsonPropertyName("latitude")] double Latitude,
    [property: JsonPropertyName("longitude")] double Longitude,
    [property: JsonPropertyName("current")] OpenMeteoCurrentConditions Current
);

internal sealed record OpenMeteoCurrentConditions(
    [property: JsonPropertyName("temperature_2m")] double Temperature,
    [property: JsonPropertyName("relative_humidity_2m")] double RelativeHumidity,
    [property: JsonPropertyName("apparent_temperature")] double ApparentTemperature,
    [property: JsonPropertyName("weather_code")] int WeatherCode,
    [property: JsonPropertyName("wind_speed_10m")] double WindSpeed,
    [property: JsonPropertyName("wind_direction_10m")] double WindDirection,
    [property: JsonPropertyName("surface_pressure")] double SurfacePressure,
    [property: JsonPropertyName("is_day")] int IsDay
);
