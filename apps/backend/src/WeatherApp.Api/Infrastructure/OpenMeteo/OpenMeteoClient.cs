using WeatherApp.Api.Features.Weather;
using WeatherApp.Api.Features.Weather.GetCurrentWeather;
using WeatherApp.Api.Features.Weather.GetForecast;
using WeatherApp.Api.Features.Weather.GetHourly;

namespace WeatherApp.Api.Infrastructure.OpenMeteo;

internal sealed class OpenMeteoClient(HttpClient httpClient) : IWeatherProvider
{
    public async Task<WeatherConditionsDto?> GetCurrentAsync(
        double lat,
        double lon,
        CancellationToken ct
    )
    {
        var url = FormattableString.Invariant(
            $"/v1/forecast?latitude={lat}&longitude={lon}&current=temperature_2m,relative_humidity_2m,apparent_temperature,weather_code,wind_speed_10m,wind_direction_10m,surface_pressure,is_day&timezone=auto"
        );

        var response = await httpClient.GetFromJsonAsync<OpenMeteoResponse>(url, ct);
        if (response is null)
            return null;

        var current = response.Current;

        return new WeatherConditionsDto(
            response.Latitude,
            response.Longitude,
            current.Temperature,
            current.WindSpeed,
            current.WeatherCode,
            current.RelativeHumidity,
            current.ApparentTemperature,
            current.WindDirection,
            current.SurfacePressure,
            current.IsDay == 1
        );
    }

    public async Task<ForecastResponse?> GetForecastAsync(
        double lat,
        double lon,
        int days,
        CancellationToken ct
    )
    {
        var url =
            $"/v1/forecast?latitude={lat}&longitude={lon}&daily=weather_code,temperature_2m_max,temperature_2m_min,precipitation_probability_max,wind_speed_10m_max,uv_index_max,sunrise,sunset&forecast_days={days}&timezone=auto";

        var response = await httpClient.GetFromJsonAsync<OpenMeteoForecastResponse>(url, ct);
        if (response is null)
            return null;

        var daily = response.Daily;

        return new ForecastResponse(
            response.Latitude,
            response.Longitude,
            [
                .. Enumerable
                    .Range(0, daily.Time.Length)
                    .Select(i => new DailyForecastDto(
                        daily.Time[i],
                        daily.WeatherCode[i],
                        daily.TemperatureMax[i],
                        daily.TemperatureMin[i],
                        daily.PrecipitationProbabilityMax[i],
                        daily.WindSpeedMax[i],
                        daily.UvIndexMax[i],
                        daily.Sunrise[i],
                        daily.Sunset[i]
                    )),
            ]
        );
    }

    public async Task<HourlyResponse?> GetHourlyAsync(
        double lat,
        double lon,
        int hours,
        CancellationToken ct
    )
    {
        var url =
            $"/v1/forecast?latitude={lat}&longitude={lon}&hourly=temperature_2m,weather_code,precipitation_probability,wind_speed_10m&forecast_hours={hours}&timezone=auto";

        var response = await httpClient.GetFromJsonAsync<OpenMeteoHourlyResponse>(url, ct);
        if (response is null)
            return null;

        var hourly = response.Hourly;

        return new HourlyResponse(
            response.Latitude,
            response.Longitude,
            [
                .. Enumerable
                    .Range(0, hourly.Time.Length)
                    .Select(i => new HourlyEntryDto(
                        hourly.Time[i],
                        hourly.Temperature[i],
                        hourly.WeatherCode[i],
                        hourly.PrecipitationProbability[i],
                        hourly.WindSpeed[i]
                    )),
            ]
        );
    }
}
