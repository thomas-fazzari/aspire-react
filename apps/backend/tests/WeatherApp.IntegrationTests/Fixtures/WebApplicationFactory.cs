using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace WeatherApp.IntegrationTests.Fixtures;

internal sealed class WebApplicationFactory(
    string postgresConnectionString,
    string redisConnectionString,
    Action<IServiceCollection> configureTestServices
) : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Development");

        builder.UseSetting("ConnectionStrings:weatherdb", postgresConnectionString);
        builder.UseSetting("ConnectionStrings:redis", redisConnectionString);
        builder.UseSetting("OpenMeteo:BaseUrl", "https://open-meteo.test");

        builder.ConfigureTestServices(services =>
        {
            configureTestServices(services);
        });
    }
}
