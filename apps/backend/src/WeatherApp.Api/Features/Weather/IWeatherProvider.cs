using WeatherApp.Api.Features.Weather.GetCurrentWeather;
using WeatherApp.Api.Features.Weather.GetForecast;
using WeatherApp.Api.Features.Weather.GetHourly;

namespace WeatherApp.Api.Features.Weather;

internal interface IWeatherProvider
{
    Task<WeatherConditionsDto?> GetCurrentAsync(double lat, double lon, CancellationToken ct);
    Task<ForecastResponse?> GetForecastAsync(
        double lat,
        double lon,
        int days,
        CancellationToken ct
    );
    Task<HourlyResponse?> GetHourlyAsync(double lat, double lon, int hours, CancellationToken ct);
}
