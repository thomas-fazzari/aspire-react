namespace WeatherApp.Api.Domain.Weather;

internal sealed record WeatherData(
    double Latitude,
    double Longitude,
    double Temperature,
    double WindSpeed,
    int WeatherCode
);
