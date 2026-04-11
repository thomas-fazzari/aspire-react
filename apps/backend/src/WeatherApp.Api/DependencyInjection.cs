using FluentValidation;
using Microsoft.EntityFrameworkCore;
using WeatherApp.Api.Features.Cities;
using WeatherApp.Api.Features.Weather;
using WeatherApp.Api.Infrastructure.Persistence;

namespace WeatherApp.Api;

/// <summary>
/// Composition root for all features services and API endpoints mappings.
/// </summary>
internal static class DependencyInjection
{
    internal static IServiceCollection AddWeatherAppApi(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddDbContext<AppDbContext>(
            (_, options) => options.UseNpgsql(configuration.GetConnectionString("weatherdb"))
        );

        services.AddValidatorsFromAssembly(
            typeof(DependencyInjection).Assembly,
            includeInternalTypes: true
        );
        services.AddCitiesFeature();
        services.AddWeatherFeature();

        return services;
    }

    internal static IEndpointRouteBuilder MapWeatherAppApi(this IEndpointRouteBuilder app)
    {
        app.MapCitiesFeature();
        app.MapWeatherFeature();

        return app;
    }
}
