using WeatherApp.Api.Features.Weather;

namespace WeatherApp.Api.Features.Weather.GetCurrentWeather;

internal sealed partial class GetCurrentWeatherHandler(
    IWeatherProvider weatherProvider,
    ILogger<GetCurrentWeatherHandler> logger
)
{
    public async Task<Result<GetCurrentWeatherResponse>> HandleAsync(
        GetCurrentWeatherRequest request,
        CancellationToken ct
    )
    {
        var weather = await weatherProvider.GetCurrentAsync(request.Lat, request.Lon, ct);

        if (weather is null)
        {
            LogWeatherFetchFailed(logger, request.Lat, request.Lon);
            return Result.Fail<GetCurrentWeatherResponse>(
                WeatherErrors.FetchFailed("Open-Meteo returned no data")
            );
        }

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
