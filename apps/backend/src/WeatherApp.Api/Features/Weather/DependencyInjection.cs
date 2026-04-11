using Microsoft.Extensions.Options;
using WeatherApp.Api.Domain.Weather;
using WeatherApp.Api.Features.Weather.GetWeather;
using WeatherApp.Api.Infrastructure.OpenMeteo;

namespace WeatherApp.Api.Features.Weather;

internal static class DependencyInjection
{
    public static IServiceCollection AddWeatherFeature(this IServiceCollection services)
    {
        services.AddScoped<GetWeatherHandler>();
        services
            .AddOptions<OpenMeteoOptions>()
            .BindConfiguration(OpenMeteoOptions.SectionName)
            .ValidateDataAnnotations()
            .ValidateOnStart();
        services
            .AddHttpClient<IWeatherProvider, OpenMeteoClient>(
                (sp, c) =>
                {
                    var options = sp.GetRequiredService<IOptions<OpenMeteoOptions>>().Value;
                    c.BaseAddress = new Uri(options.BaseUrl);
                }
            )
            .AddStandardResilienceHandler();

        return services;
    }

    public static IEndpointRouteBuilder MapWeatherFeature(this IEndpointRouteBuilder app)
    {
        app.MapWeatherEndpoints();

        return app;
    }
}
