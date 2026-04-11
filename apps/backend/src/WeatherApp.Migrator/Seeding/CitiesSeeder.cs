using Microsoft.EntityFrameworkCore;
using WeatherApp.Api.Domain.Cities;
using WeatherApp.Api.Infrastructure.Persistence;

namespace WeatherApp.Migrator;

internal static class CitiesSeeder
{
    private static readonly City[] _seeds =
    [
        City.Create("Paris", 48.8566, 2.3522, "FR"),
        City.Create("London", 51.5074, -0.1278, "GB"),
        City.Create("Tokyo", 35.6762, 139.6503, "JP"),
        City.Create("New York", 40.7128, -74.0060, "US"),
        City.Create("Sydney", -33.8688, 151.2093, "AU"),
        City.Create("Berlin", 52.5200, 13.4050, "DE"),
        City.Create("Cairo", 30.0444, 31.2357, "EG"),
        City.Create("Mexico City", 19.4326, -99.1332, "MX"),
        City.Create("Mumbai", 19.0760, 72.8777, "IN"),
        City.Create("São Paulo", -23.5505, -46.6333, "BR"),
        City.Create("Moscow", 55.7558, 37.6173, "RU"),
        City.Create("Seoul", 37.5665, 126.9780, "KR"),
        City.Create("Buenos Aires", -34.6037, -58.3816, "AR"),
        City.Create("Lagos", 6.5244, 3.3792, "NG"),
        City.Create("Istanbul", 41.0082, 28.9784, "TR"),
        City.Create("Beijing", 39.9042, 116.4074, "CN"),
        City.Create("Nairobi", -1.2921, 36.8219, "KE"),
        City.Create("Toronto", 43.6532, -79.3832, "CA"),
        City.Create("Dubai", 25.2048, 55.2708, "AE"),
        City.Create("Reykjavik", 64.1466, -21.9426, "IS"),
    ];

    public static async Task SeedAsync(AppDbContext dbContext, CancellationToken ct)
    {
        if (await dbContext.Cities.AnyAsync(ct))
            return;

        dbContext.Cities.AddRange(_seeds);
        await dbContext.SaveChangesAsync(ct);
    }
}
