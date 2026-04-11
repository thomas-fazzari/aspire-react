using WeatherApp.Api.Domain.Weather;

namespace WeatherApp.Api.Infrastructure.OpenMeteo;

internal sealed class OpenMeteoClient(HttpClient httpClient) : IWeatherProvider
{
    public async Task<WeatherData?> GetCurrentAsync(double lat, double lon, CancellationToken ct)
    {
        var url = FormattableString.Invariant(
            $"/v1/forecast?latitude={lat}&longitude={lon}&current=temperature_2m,wind_speed_10m,weather_code"
        );

        var response = await httpClient.GetFromJsonAsync<OpenMeteoResponse>(url, ct);
        if (response is null)
            return null;

        var current = response.Current;

        return new WeatherData(
            response.Latitude,
            response.Longitude,
            current.Temperature,
            current.WindSpeed,
            current.WeatherCode
        );
    }
}
