using WeatherApp.Api.Shared.Http;

namespace WeatherApp.Api.Features.Weather.GetForecast;

internal static class ForecastEndpoints
{
    public static IEndpointRouteBuilder MapGetForecastEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/weather/forecast", GetForecast)
            .WithName("GetForecast")
            .WithTags("Weather")
            .WithValidation<GetForecastRequest>()
            .CacheOutput(p => p.Expire(TimeSpan.FromMinutes(30)));

        return app;
    }

    private static async Task<Results<Ok<ForecastResponse>, ProblemHttpResult>> GetForecast(
        [AsParameters] GetForecastRequest request,
        GetForecastHandler handler,
        CancellationToken ct
    )
    {
        var result = await handler.HandleAsync(request, ct);
        return result.ToHttpResult();
    }
}
