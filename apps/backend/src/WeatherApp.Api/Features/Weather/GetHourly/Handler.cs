using WeatherApp.Api.Features.Weather;

namespace WeatherApp.Api.Features.Weather.GetHourly;

internal sealed partial class GetHourlyHandler(
    IWeatherProvider weatherProvider,
    ILogger<GetHourlyHandler> logger
)
{
    public async Task<Result<HourlyResponse>> HandleAsync(
        GetHourlyRequest request,
        CancellationToken ct
    )
    {
        var hourly = await weatherProvider.GetHourlyAsync(
            request.Lat,
            request.Lon,
            request.Hours,
            ct
        );

        if (hourly is null)
        {
            LogHourlyFetchFailed(logger, request.Lat, request.Lon);
            return Result.Fail<HourlyResponse>(
                WeatherErrors.FetchFailed("Open-Meteo returned no hourly data")
            );
        }

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
