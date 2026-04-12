using System.Diagnostics;
using WeatherApp.Api.Infrastructure.Metrics;

namespace WeatherApp.Api.Features.Weather.GetForecast;

internal sealed partial class GetForecastHandler(
    IWeatherProvider weatherProvider,
    AppMetrics appMetrics,
    ILogger<GetForecastHandler> logger
)
{
    public async Task<Result<ForecastResponse>> HandleAsync(
        GetForecastRequest request,
        CancellationToken ct
    )
    {
        appMetrics.Request(AppMetrics.Endpoints.WeatherForecast);

        var startTime = Stopwatch.GetTimestamp();
        var forecast = await weatherProvider.GetForecastAsync(
            request.Lat,
            request.Lon,
            request.Days,
            ct
        );
        var duration = Stopwatch.GetElapsedTime(startTime);

        if (forecast is null)
        {
            appMetrics.OpenMeteoCallFailed(duration);
            LogForecastFetchFailed(logger, request.Lat, request.Lon);
            return Result.Fail<ForecastResponse>(
                WeatherErrors.FetchFailed("Open-Meteo returned no forecast data")
            );
        }

        appMetrics.OpenMeteoCallSucceeded(duration);
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
