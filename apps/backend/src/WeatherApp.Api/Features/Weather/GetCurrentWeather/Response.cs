namespace WeatherApp.Api.Features.Weather.GetCurrentWeather;

internal sealed record WeatherConditionsDto(
    double Lat,
    double Lon,
    double Temperature,
    double WindSpeed,
    int WeatherCode,
    double RelativeHumidity,
    double ApparentTemperature,
    double WindDirection,
    double SurfacePressure,
    bool IsDay
);

internal sealed record GetCurrentWeatherResponse(
    double Lat,
    double Lon,
    WeatherConditionsDto Current
);
