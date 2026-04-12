using System.Net;
using Aspire.Hosting.Testing;

namespace WeatherApp.AspireTests;

[CollectionDefinition("Aspire")]
public class AspireCollection : ICollectionFixture<AppHostFixture>;

[Collection("Aspire")]
[Trait("Category", "Aspire")]
public sealed class AppHostTests(AppHostFixture fixture)
{
    private const string ApiResourceName = "api";

    [Fact]
    public async Task Api_BecomesHealthy()
    {
        var ct = TestContext.Current.CancellationToken;
        using var cts = CancellationTokenSource.CreateLinkedTokenSource(ct);
        cts.CancelAfter(TimeSpan.FromMinutes(2));

        await fixture.App.ResourceNotifications.WaitForResourceHealthyAsync(
            ApiResourceName,
            cts.Token
        );
    }

    [Fact]
    public async Task Api_RespondsToAliveEndpoint()
    {
        var ct = TestContext.Current.CancellationToken;
        using var cts = CancellationTokenSource.CreateLinkedTokenSource(ct);
        cts.CancelAfter(TimeSpan.FromMinutes(2));

        await fixture.App.ResourceNotifications.WaitForResourceHealthyAsync(
            ApiResourceName,
            cts.Token
        );

        var httpClient = fixture.App.CreateHttpClient(ApiResourceName);
        var response = await httpClient.GetAsync("/alive", cts.Token);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
