using Aspire.Hosting;
using Aspire.Hosting.Testing;

namespace WeatherApp.AspireTests;

public sealed class AppHostFixture : IAsyncLifetime
{
    private DistributedApplication? _app;

    public DistributedApplication App =>
        _app ?? throw new InvalidOperationException("App not initialized");

    public async ValueTask InitializeAsync()
    {
        var ct = TestContext.Current.CancellationToken;
        var builder =
            await DistributedApplicationTestingBuilder.CreateAsync<Projects.WeatherApp_Host>(
                [
                    "AppHost:TestMode=true",
                    "Parameters:postgres-password=TestPassword123!",
                    "Parameters:open-meteo-base-url=https://api.open-meteo.com",
                ],
                ct
            );

        _app = await builder.BuildAsync(ct);

        using var startupCts = CancellationTokenSource.CreateLinkedTokenSource(ct);
        startupCts.CancelAfter(TimeSpan.FromMinutes(5));
        try
        {
            await _app.StartAsync(startupCts.Token);
        }
        catch
        {
            await DisposeAsync();
            throw;
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_app is not null)
        {
            await _app.DisposeAsync();
            _app = null;
        }
    }
}
