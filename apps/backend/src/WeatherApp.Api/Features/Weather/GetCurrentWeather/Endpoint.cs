using WeatherApp.Api.Shared.Http;

namespace WeatherApp.Api.Features.Weather.GetCurrentWeather;

internal static class GetCurrentWeatherEndpoints
{
    public static IEndpointRouteBuilder MapGetCurrentWeatherEndpoints(
        this IEndpointRouteBuilder app
    )
    {
        app.MapGet("/weather/current", GetCurrentWeather)
            .WithName("GetCurrentWeather")
            .WithTags("Weather")
            .WithValidation<GetCurrentWeatherRequest>()
            .CacheOutput(p => p.Expire(TimeSpan.FromMinutes(10)));

        return app;
    }

    private static async Task<
        Results<Ok<GetCurrentWeatherResponse>, ProblemHttpResult>
    > GetCurrentWeather(
        [AsParameters] GetCurrentWeatherRequest request,
        GetCurrentWeatherHandler handler,
        CancellationToken ct
    )
    {
        var result = await handler.HandleAsync(request, ct);
        return result.ToHttpResult();
    }
}
