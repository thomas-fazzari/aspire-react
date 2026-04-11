using System.Net;
using Moq;
using WeatherApp.IntegrationTests.Fixtures;

namespace WeatherApp.IntegrationTests.Weather.GetHourly;

[Collection("Weather")]
public sealed class EndpointTests(ApiFixture fixture) : WeatherIntegrationTests(fixture)
{
    [Fact]
    public async Task WithValidCoordinates_ReturnsOk()
    {
        using var client = Fixture.CreateApiClient();
        var ct = TestContext.Current.CancellationToken;

        const string requestPath = "/weather/hourly?lat=48.85&lon=2.35";
        var response = await client.GetAsync(requestPath, ct);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task WithValidCoordinatesAndHours_ReturnsOk()
    {
        using var client = Fixture.CreateApiClient();
        var ct = TestContext.Current.CancellationToken;

        const string requestPath = "/weather/hourly?lat=48.85&lon=2.35&hours=12";
        var response = await client.GetAsync(requestPath, ct);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task WithInvalidCoordinates_ReturnsValidationErrorWithoutCallingProvider()
    {
        using var client = Fixture.CreateApiClient();
        var ct = TestContext.Current.CancellationToken;

        const string requestPath = "/weather/hourly?lat=999&lon=999";
        Fixture.WeatherProviderMock.Invocations.Clear();

        var response = await client.GetAsync(requestPath, ct);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Fixture.WeatherProviderMock.Verify(
            provider =>
                provider.GetHourlyAsync(
                    It.IsAny<double>(),
                    It.IsAny<double>(),
                    It.IsAny<int>(),
                    It.IsAny<CancellationToken>()
                ),
            Times.Never()
        );
        Fixture.WeatherProviderMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task WithValidCoordinates_CachesResponse()
    {
        using var client = Fixture.CreateApiClient();
        var ct = TestContext.Current.CancellationToken;
        const string requestPath = "/weather/hourly?lat=51.50&lon=-0.12";

        Fixture.WeatherProviderMock.Invocations.Clear();

        var firstResponse = await client.GetAsync(requestPath, ct);
        var secondResponse = await client.GetAsync(requestPath, ct);

        Assert.Equal(HttpStatusCode.OK, firstResponse.StatusCode);
        Assert.Equal(HttpStatusCode.OK, secondResponse.StatusCode);

        Fixture.WeatherProviderMock.Verify(
            provider =>
                provider.GetHourlyAsync(
                    It.IsAny<double>(),
                    It.IsAny<double>(),
                    It.IsAny<int>(),
                    It.IsAny<CancellationToken>()
                ),
            Times.Once()
        );
        Fixture.WeatherProviderMock.VerifyNoOtherCalls();
    }
}
