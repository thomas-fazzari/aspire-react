using Microsoft.EntityFrameworkCore;
using WeatherApp.Api.Infrastructure.Metrics;
using WeatherApp.Api.Infrastructure.Persistence;

namespace WeatherApp.Api.Features.Cities.GetCities;

internal sealed class GetCitiesHandler(AppDbContext dbContext, AppMetrics appMetrics)
{
    public Task<CityResponse[]> HandleAsync(CancellationToken ct)
    {
        appMetrics.Request(AppMetrics.Endpoints.Cities);

        return dbContext
            .Cities.OrderBy(_ => EF.Functions.Random())
            .Take(5)
            .Select(c => new CityResponse(c.Name, c.Lat, c.Lon, c.CountryCode))
            .ToArrayAsync(ct);
    }
}
