using System.Net;
using Moq;
using WeatherApp.IntegrationTests.Fixtures;

namespace WeatherApp.IntegrationTests.Weather.GetWeather;

public sealed class EndpointTests(ApiFixture fixture)
{
    [Fact]
    public async Task WithValidCoordinates_ReturnsOk()
    {
        using var client = fixture.CreateApiClient();
        var ct = TestContext.Current.CancellationToken;

        const string requestPath = "/weather?lat=48.85&lon=2.35";
        var response = await client.GetAsync(requestPath, ct);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task WithInvalidCoordinates_ReturnsValidationErrorWithoutCallingProvider()
    {
        using var client = fixture.CreateApiClient();
        var ct = TestContext.Current.CancellationToken;

        const string requestPath = "/weather?lat=999&lon=999";
        fixture.WeatherProviderMock.Invocations.Clear();

        var response = await client.GetAsync(requestPath, ct);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        fixture.WeatherProviderMock.Verify(
            provider =>
                provider.GetCurrentAsync(
                    It.IsAny<double>(),
                    It.IsAny<double>(),
                    It.IsAny<CancellationToken>()
                ),
            Times.Never()
        );
        fixture.WeatherProviderMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task WithValidCoordinates_CachesResponse()
    {
        using var client = fixture.CreateApiClient();
        var ct = TestContext.Current.CancellationToken;
        const string requestPath = "/weather?lat=51.50&lon=-0.12";

        fixture.WeatherProviderMock.Invocations.Clear();

        var firstResponse = await client.GetAsync(requestPath, ct);
        var secondResponse = await client.GetAsync(requestPath, ct);

        Assert.Equal(HttpStatusCode.OK, firstResponse.StatusCode);
        Assert.Equal(HttpStatusCode.OK, secondResponse.StatusCode);

        fixture.WeatherProviderMock.Verify(
            provider =>
                provider.GetCurrentAsync(
                    It.IsAny<double>(),
                    It.IsAny<double>(),
                    It.IsAny<CancellationToken>()
                ),
            Times.Once()
        );
        fixture.WeatherProviderMock.VerifyNoOtherCalls();
    }
}
