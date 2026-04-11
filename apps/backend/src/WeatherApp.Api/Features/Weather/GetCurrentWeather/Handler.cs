using System.Diagnostics;

namespace WeatherApp.Api.Features.Weather.GetCurrentWeather;

internal sealed partial class GetCurrentWeatherHandler(
    IWeatherProvider weatherProvider,
    WeatherMetrics metrics,
    ILogger<GetCurrentWeatherHandler> logger
)
{
    public async Task<Result<GetCurrentWeatherResponse>> HandleAsync(
        GetCurrentWeatherRequest request,
        CancellationToken ct
    )
    {
        metrics.RequestCalled(WeatherMetrics.Endpoints.Current);

        var stopwatch = Stopwatch.StartNew();
        var weather = await weatherProvider.GetCurrentAsync(request.Lat, request.Lon, ct);
        stopwatch.Stop();

        if (weather is null)
        {
            LogWeatherFetchFailed(logger, request.Lat, request.Lon);
            metrics.ProviderCallFailed(
                WeatherMetrics.Endpoints.Current,
                stopwatch.Elapsed.TotalMilliseconds
            );
            return Result.Fail<GetCurrentWeatherResponse>(
                WeatherErrors.FetchFailed("Open-Meteo returned no data")
            );
        }

        metrics.ProviderCallSucceeded(
            WeatherMetrics.Endpoints.Current,
            stopwatch.Elapsed.TotalMilliseconds
        );
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
