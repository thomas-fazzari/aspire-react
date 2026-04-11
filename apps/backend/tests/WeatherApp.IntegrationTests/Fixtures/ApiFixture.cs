extern alias Migrator;
using Moq;
using Testcontainers.PostgreSql;
using Testcontainers.Redis;

using WeatherApp.Api.Domain.Weather;
using WeatherApp.TestHelpers.Mocks;

using DatabaseMigrationRunner = Migrator::WeatherApp.Migrator.DatabaseMigrationRunner;
using WeatherProviderMockFactory = WeatherApp.TestHelpers.Mocks.WeatherProviderMock;

[assembly: AssemblyFixture(typeof(WeatherApp.IntegrationTests.Fixtures.ApiFixture))]

namespace WeatherApp.IntegrationTests.Fixtures;

public sealed class ApiFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgres = new PostgreSqlBuilder("postgres:17-alpine")
        .WithDatabase("weather-app-tests")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .Build();

    private readonly RedisContainer _redis = new RedisBuilder("redis:8-alpine").Build();

    private WebApplicationFactory? _factory;
    private Mock<IWeatherProvider>? _weatherProviderMock;

    private WebApplicationFactory Factory =>
        _factory
        ?? throw new InvalidOperationException(
            "Test API host not initialized. Did InitializeAsync complete?"
        );

    internal Mock<IWeatherProvider> WeatherProviderMock =>
        _weatherProviderMock
        ?? throw new InvalidOperationException(
            "Weather provider mock not initialized. Did InitializeAsync complete?"
        );

    public HttpClient CreateApiClient() => Factory.CreateClient();

    public async ValueTask InitializeAsync()
    {
        await Task.WhenAll(_postgres.StartAsync(), _redis.StartAsync());

        _weatherProviderMock = WeatherProviderMockFactory.Create();

        _factory = new WebApplicationFactory(
            _postgres.GetConnectionString(),
            _redis.GetConnectionString(),
            services => services.AddMock(WeatherProviderMock)
        );

        await DatabaseMigrationRunner.InitializeAsync(Factory.Services, CancellationToken.None);
    }

    public async ValueTask DisposeAsync()
    {
        _factory?.Dispose();
        await Task.WhenAll(_redis.DisposeAsync().AsTask(), _postgres.DisposeAsync().AsTask());
    }
}
