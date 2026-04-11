using WeatherApp.Api.Features.Weather.GetForecast;

namespace WeatherApp.UnitTests.Weather.GetForecast;

public sealed class ValidatorTests
{
    private readonly GetForecastRequestValidator _sut = new();

    [Theory]
    [InlineData(48.85, 2.35, 1)]
    [InlineData(48.85, 2.35, 7)]
    [InlineData(48.85, 2.35, 16)]
    [InlineData(-90, -180, 1)]
    [InlineData(90, 180, 16)]
    public void Validate_WithValidParameters_ReturnsValid(double lat, double lon, int days)
    {
        var result = _sut.Validate(new GetForecastRequest(lat, lon, days));

        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData(-91, 0, 7, nameof(GetForecastRequest.Lat))]
    [InlineData(91, 0, 7, nameof(GetForecastRequest.Lat))]
    [InlineData(0, -181, 7, nameof(GetForecastRequest.Lon))]
    [InlineData(0, 181, 7, nameof(GetForecastRequest.Lon))]
    [InlineData(48.85, 2.35, 0, nameof(GetForecastRequest.Days))]
    [InlineData(48.85, 2.35, 17, nameof(GetForecastRequest.Days))]
    public void Validate_WithInvalidParameters_ReturnsValidationError(
        double lat,
        double lon,
        int days,
        string propertyName
    )
    {
        var result = _sut.Validate(new GetForecastRequest(lat, lon, days));

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, error => error.PropertyName == propertyName);
    }
}
