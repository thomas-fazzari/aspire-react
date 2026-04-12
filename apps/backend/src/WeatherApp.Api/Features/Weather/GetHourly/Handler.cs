using System.Diagnostics;
using WeatherApp.Api.Infrastructure.Metrics;

namespace WeatherApp.Api.Features.Weather.GetHourly;

internal sealed partial class GetHourlyHandler(
    IWeatherProvider weatherProvider,
    AppMetrics appMetrics,
    ILogger<GetHourlyHandler> logger
)
{
    public async Task<Result<HourlyResponse>> HandleAsync(
        GetHourlyRequest request,
        CancellationToken ct
    )
    {
        appMetrics.Request(AppMetrics.Endpoints.WeatherHourly);

        var startTime = Stopwatch.GetTimestamp();
        var hourly = await weatherProvider.GetHourlyAsync(
            request.Lat,
            request.Lon,
            request.Hours,
            ct
        );
        var duration = Stopwatch.GetElapsedTime(startTime);

        if (hourly is null)
        {
            appMetrics.OpenMeteoCallFailed(duration);
            LogHourlyFetchFailed(logger, request.Lat, request.Lon);
            return Result.Fail<HourlyResponse>(
                WeatherErrors.FetchFailed("Open-Meteo returned no hourly data")
            );
        }

        appMetrics.OpenMeteoCallSucceeded(duration);
        LogFetchedHourly(logger, request.Lat, request.Lon, request.Hours);

        return Result.Ok(hourly);
    }

    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Fetched {Hours}-hour forecast for {Lat},{Lon}"
    )]
    private static partial void LogFetchedHourly(ILogger logger, double lat, double lon, int hours);

    [LoggerMessage(
        Level = LogLevel.Warning,
        Message = "Open-Meteo returned no hourly data for {Lat},{Lon}"
    )]
    private static partial void LogHourlyFetchFailed(ILogger logger, double lat, double lon);
}
