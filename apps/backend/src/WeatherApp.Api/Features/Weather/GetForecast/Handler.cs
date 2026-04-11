using System.Diagnostics;

namespace WeatherApp.Api.Features.Weather.GetForecast;

internal sealed partial class GetForecastHandler(
    IWeatherProvider weatherProvider,
    WeatherMetrics metrics,
    ILogger<GetForecastHandler> logger
)
{
    public async Task<Result<ForecastResponse>> HandleAsync(
        GetForecastRequest request,
        CancellationToken ct
    )
    {
        metrics.RequestCalled(WeatherMetrics.Endpoints.Forecast);

        var stopwatch = Stopwatch.StartNew();
        var forecast = await weatherProvider.GetForecastAsync(
            request.Lat,
            request.Lon,
            request.Days,
            ct
        );
        stopwatch.Stop();

        if (forecast is null)
        {
            LogForecastFetchFailed(logger, request.Lat, request.Lon);
            metrics.ProviderCallFailed(
                WeatherMetrics.Endpoints.Forecast,
                stopwatch.Elapsed.TotalMilliseconds
            );
            return Result.Fail<ForecastResponse>(
                WeatherErrors.FetchFailed("Open-Meteo returned no forecast data")
            );
        }

        metrics.ProviderCallSucceeded(
            WeatherMetrics.Endpoints.Forecast,
            stopwatch.Elapsed.TotalMilliseconds
        );
        LogFetchedForecast(logger, request.Lat, request.Lon, request.Days);

        return Result.Ok(forecast);
    }

    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Fetched {Days}-day forecast for {Lat},{Lon}"
    )]
    private static partial void LogFetchedForecast(
        ILogger logger,
        double lat,
        double lon,
        int days
    );

    [LoggerMessage(
        Level = LogLevel.Warning,
        Message = "Open-Meteo returned no forecast data for {Lat},{Lon}"
    )]
    private static partial void LogForecastFetchFailed(ILogger logger, double lat, double lon);
}
