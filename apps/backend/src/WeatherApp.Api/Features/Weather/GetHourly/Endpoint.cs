using WeatherApp.Api.Shared.Http;

namespace WeatherApp.Api.Features.Weather.GetHourly;

internal static class HourlyEndpoints
{
    public static IEndpointRouteBuilder MapGetHourlyEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/weather/hourly", GetHourly)
            .WithName("GetHourly")
            .WithTags("Weather")
            .WithValidation<GetHourlyRequest>()
            .CacheOutput(p => p.Expire(TimeSpan.FromMinutes(15)));

        return app;
    }

    private static async Task<Results<Ok<HourlyResponse>, ProblemHttpResult>> GetHourly(
        [AsParameters] GetHourlyRequest request,
        GetHourlyHandler handler,
        CancellationToken ct
    )
    {
        var result = await handler.HandleAsync(request, ct);
        return result.ToHttpResult();
    }
}
