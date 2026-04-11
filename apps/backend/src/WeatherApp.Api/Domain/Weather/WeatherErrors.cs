using WeatherApp.Api.Shared.Http;

namespace WeatherApp.Api.Domain.Weather;

internal static class WeatherErrors
{
    public static IError FetchFailed(string detail) =>
        WeatherAppError.Failure(
            code: WeatherErrorCodes.FetchFailed,
            message: $"Could not fetch weather: {detail}"
        );
}
