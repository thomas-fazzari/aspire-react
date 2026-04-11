namespace WeatherApp.Api.Domain.Weather;

internal interface IWeatherProvider
{
    Task<WeatherData?> GetCurrentAsync(double lat, double lon, CancellationToken ct);
}
