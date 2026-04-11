namespace WeatherApp.Api.Features.Weather.GetForecast;

internal sealed record ForecastResponse(double Lat, double Lon, DailyForecastDto[] Days);

internal sealed record DailyForecastDto(
    string Date,
    int WeatherCode,
    double TemperatureMax,
    double TemperatureMin,
    double PrecipitationProbability,
    double WindSpeedMax,
    double UvIndex,
    string Sunrise,
    string Sunset
);
