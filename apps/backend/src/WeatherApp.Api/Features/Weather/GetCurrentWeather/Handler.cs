using System.Diagnostics;
using WeatherApp.Api.Infrastructure.Metrics;

namespace WeatherApp.Api.Features.Weather.GetCurrentWeather;

internal sealed partial class GetCurrentWeatherHandler(
    IWeatherProvider weatherProvider,
    AppMetrics appMetrics,
    ILogger<GetCurrentWeatherHandler> logger
)
{
    public async Task<Result<GetCurrentWeatherResponse>> HandleAsync(
        GetCurrentWeatherRequest request,
        CancellationToken ct
    )
    {
        appMetrics.Request(AppMetrics.Endpoints.WeatherCurrent);

        var startTime = Stopwatch.GetTimestamp();
        var weather = await weatherProvider.GetCurrentAsync(request.Lat, request.Lon, ct);
        var duration = Stopwatch.GetElapsedTime(startTime);

        if (weather is null)
        {
            appMetrics.OpenMeteoCallFailed(duration);
            LogWeatherFetchFailed(logger, request.Lat, request.Lon);
            return Result.Fail<GetCurrentWeatherResponse>(
                WeatherErrors.FetchFailed("Open-Meteo returned no data")
            );
        }

        appMetrics.OpenMeteoCallSucceeded(duration);
        LogFetchedWeather(logger, request.Lat, request.Lon);

        return Result.Ok(new GetCurrentWeatherResponse(weather.Lat, weather.Lon, weather));
    }

    [LoggerMessage(Level = LogLevel.Information, Message = "Fetched weather for {Lat},{Lon}")]
    private static partial void LogFetchedWeather(ILogger logger, double lat, double lon);

    [LoggerMessage(
        Level = LogLevel.Warning,
        Message = "Open-Meteo returned no data for {Lat},{Lon}"
    )]
    private static partial void LogWeatherFetchFailed(ILogger logger, double lat, double lon);
}
