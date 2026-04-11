using Moq;
using WeatherApp.Api.Domain.Weather;
using WeatherApp.TestHelpers.Fakers;

namespace WeatherApp.TestHelpers.Mocks;

internal static class WeatherProviderMock
{
    /// <summary>
    /// Creates a mock that returns generated weather data for any coordinate pair
    /// </summary>
    internal static Mock<IWeatherProvider> Create()
    {
        var mock = new Mock<IWeatherProvider>();

        mock.Setup(provider =>
                provider.GetCurrentAsync(
                    It.IsAny<double>(),
                    It.IsAny<double>(),
                    It.IsAny<CancellationToken>()
                )
            )
            .ReturnsAsync(WeatherDataFaker.Generate());

        return mock;
    }
}
