using Microsoft.EntityFrameworkCore;
using WeatherApp.Api.Infrastructure.Persistence;

namespace WeatherApp.Migrator;

internal static class DatabaseMigrationRunner
{
    internal static async Task InitializeAsync(IServiceProvider services, CancellationToken ct)
    {
        using var scope = services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var strategy = dbContext.Database.CreateExecutionStrategy();

        await strategy.ExecuteAsync(async () =>
        {
            await dbContext.Database.MigrateAsync(ct);
            await CitiesSeeder.SeedAsync(dbContext, ct);
        });
    }
}
