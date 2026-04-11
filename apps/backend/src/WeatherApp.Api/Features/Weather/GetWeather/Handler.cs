using WeatherApp.Api.Domain.Weather;

namespace WeatherApp.Api.Features.Weather.GetWeather;

internal sealed partial class GetWeatherHandler(
    IWeatherProvider weatherProvider,
    ILogger<GetWeatherHandler> logger
)
{
    public async Task<Result<WeatherResponse>> HandleAsync(
        GetWeatherRequest request,
        CancellationToken ct
    )
    {
        var weather = await weatherProvider.GetCurrentAsync(request.Lat, request.Lon, ct);

        if (weather is null)
        {
            LogWeatherFetchFailed(logger, request.Lat, request.Lon);
            return Result.Fail<WeatherResponse>(
                WeatherErrors.FetchFailed("Open-Meteo returned no data")
            );
        }

        LogFetchedWeather(logger, request.Lat, request.Lon);

        return Result.Ok(
            new WeatherResponse(
                weather.Latitude,
                weather.Longitude,
                new CurrentConditions(weather.Temperature, weather.WindSpeed, weather.WeatherCode)
            )
        );
    }

    [LoggerMessage(Level = LogLevel.Information, Message = "Fetched weather for {Lat},{Lon}")]
    private static partial void LogFetchedWeather(ILogger logger, double lat, double lon);

    [LoggerMessage(
        Level = LogLevel.Warning,
        Message = "Open-Meteo returned no data for {Lat},{Lon}"
    )]
    private static partial void LogWeatherFetchFailed(ILogger logger, double lat, double lon);
}
