namespace WeatherApp.Migrator;

internal sealed partial class MigrationWorker(
    IServiceProvider serviceProvider,
    IHostApplicationLifetime hostApplicationLifetime,
    ILogger<MigrationWorker> logger
) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            await DatabaseMigrationRunner.InitializeAsync(serviceProvider, stoppingToken);
        }
        catch (Exception ex)
        {
            LogFailed(logger, ex);
            throw;
        }
        finally
        {
            hostApplicationLifetime.StopApplication();
        }
    }

    [LoggerMessage(Level = LogLevel.Error, Message = "Migration service failed")]
    private static partial void LogFailed(ILogger logger, Exception ex);
}
