using System.Diagnostics;

namespace WeatherApp.Api.Features.Weather.GetHourly;

internal sealed partial class GetHourlyHandler(
    IWeatherProvider weatherProvider,
    WeatherMetrics metrics,
    ILogger<GetHourlyHandler> logger
)
{
    public async Task<Result<HourlyResponse>> HandleAsync(
        GetHourlyRequest request,
        CancellationToken ct
    )
    {
        metrics.RequestCalled(WeatherMetrics.Endpoints.Hourly);

        var stopwatch = Stopwatch.StartNew();
        var hourly = await weatherProvider.GetHourlyAsync(
            request.Lat,
            request.Lon,
            request.Hours,
            ct
        );
        stopwatch.Stop();

        if (hourly is null)
        {
            LogHourlyFetchFailed(logger, request.Lat, request.Lon);
            metrics.ProviderCallFailed(
                WeatherMetrics.Endpoints.Hourly,
                stopwatch.Elapsed.TotalMilliseconds
            );
            return Result.Fail<HourlyResponse>(
                WeatherErrors.FetchFailed("Open-Meteo returned no hourly data")
            );
        }

        metrics.ProviderCallSucceeded(
            WeatherMetrics.Endpoints.Hourly,
            stopwatch.Elapsed.TotalMilliseconds
        );
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
