namespace WeatherApp.Api.Features.Cities.GetCities;

internal static class CitiesEndpoints
{
    public static IEndpointRouteBuilder MapCitiesEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/cities", GetCities).WithName("GetCities").WithTags("Cities");

        return app;
    }

    private static async Task<Ok<CityResponse[]>> GetCities(
        GetCitiesHandler handler,
        CancellationToken ct
    ) => TypedResults.Ok(await handler.HandleAsync(ct));
}
