using WeatherApp.Api.Features.Weather.GetWeather;

namespace WeatherApp.IntegrationTests.Weather.GetWeather;

public sealed class ValidatorTests
{
    private readonly GetWeatherRequestValidator _sut = new();

    [Theory]
    [InlineData(48.85, 2.35)]
    [InlineData(-90, -180)]
    [InlineData(90, 180)]
    public void Validate_WithCoordinatesInRange_ReturnsValid(double lat, double lon)
    {
        var result = _sut.Validate(new GetWeatherRequest(lat, lon));

        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData(-91, 0, nameof(GetWeatherRequest.Lat))]
    [InlineData(91, 0, nameof(GetWeatherRequest.Lat))]
    [InlineData(0, -181, nameof(GetWeatherRequest.Lon))]
    [InlineData(0, 181, nameof(GetWeatherRequest.Lon))]
    public void Validate_WithCoordinatesOutOfRange_ReturnsValidationError(
        double lat,
        double lon,
        string propertyName
    )
    {
        var result = _sut.Validate(new GetWeatherRequest(lat, lon));

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, error => error.PropertyName == propertyName);
    }
}
