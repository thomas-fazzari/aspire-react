using Microsoft.Extensions.Options;
using WeatherApp.Api.Features.Weather.GetCurrentWeather;
using WeatherApp.Api.Features.Weather.GetForecast;
using WeatherApp.Api.Features.Weather.GetHourly;
using WeatherApp.Api.Infrastructure.OpenMeteo;

namespace WeatherApp.Api.Features.Weather;

internal static class DependencyInjection
{
    public static IServiceCollection AddWeatherFeature(this IServiceCollection services)
    {
        services.AddScoped<GetCurrentWeatherHandler>();
        services.AddScoped<GetForecastHandler>();
        services.AddScoped<GetHourlyHandler>();
        services
            .AddOptions<OpenMeteoOptions>()
            .BindConfiguration(OpenMeteoOptions.SectionName)
            .ValidateDataAnnotations()
            .ValidateOnStart();
        services.AddHttpClient<IWeatherProvider, OpenMeteoClient>(
            (sp, c) =>
            {
                var options = sp.GetRequiredService<IOptions<OpenMeteoOptions>>().Value;
                c.BaseAddress = new Uri(options.BaseUrl);
            }
        );

        return services;
    }

    public static IEndpointRouteBuilder MapWeatherFeature(this IEndpointRouteBuilder app)
    {
        app.MapGetCurrentWeatherEndpoints();
        app.MapGetForecastEndpoints();
        app.MapGetHourlyEndpoints();

        return app;
    }
}
