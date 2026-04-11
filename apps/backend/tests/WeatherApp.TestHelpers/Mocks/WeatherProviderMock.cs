using Moq;
using WeatherApp.Api.Features.Weather;
using WeatherApp.TestHelpers.Fakers;

namespace WeatherApp.TestHelpers.Mocks;

internal static class WeatherProviderMock
{
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
            .ReturnsAsync(WeatherConditionsFaker.Generate());

        mock.Setup(provider =>
                provider.GetForecastAsync(
                    It.IsAny<double>(),
                    It.IsAny<double>(),
                    It.IsAny<int>(),
                    It.IsAny<CancellationToken>()
                )
            )
            .ReturnsAsync(ForecastFaker.Generate());

        mock.Setup(provider =>
                provider.GetHourlyAsync(
                    It.IsAny<double>(),
                    It.IsAny<double>(),
                    It.IsAny<int>(),
                    It.IsAny<CancellationToken>()
                )
            )
            .ReturnsAsync(HourlyFaker.Generate());

        return mock;
    }
}
