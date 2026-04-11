using WeatherApp.Api.Shared.Http;

namespace WeatherApp.Api.Features.Weather.GetWeather;

internal static class WeatherEndpoints
{
    public static IEndpointRouteBuilder MapWeatherEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/weather", GetWeather)
            .WithName("GetWeather")
            .WithTags("Weather")
            .WithValidation<GetWeatherRequest>()
            .CacheOutput(p => p.Expire(TimeSpan.FromMinutes(10)));

        return app;
    }

    private static async Task<Results<Ok<WeatherResponse>, ProblemHttpResult>> GetWeather(
        [AsParameters] GetWeatherRequest request,
        GetWeatherHandler handler,
        CancellationToken ct
    )
    {
        var result = await handler.HandleAsync(request, ct);
        return result.ToHttpResult();
    }
}
