using WeatherApp.Api.Features.Cities.GetCities;

namespace WeatherApp.Api.Features.Cities;

internal static class DependencyInjection
{
    public static IServiceCollection AddCitiesFeature(this IServiceCollection services)
    {
        services.AddScoped<GetCitiesHandler>();

        return services;
    }

    public static IEndpointRouteBuilder MapCitiesFeature(this IEndpointRouteBuilder app)
    {
        app.MapCitiesEndpoints();

        return app;
    }
}
