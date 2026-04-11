using WeatherApp.Api.Features.Weather.GetHourly;

namespace WeatherApp.IntegrationTests.Weather.GetHourly;

public sealed class ValidatorTests
{
    private readonly GetHourlyRequestValidator _sut = new();

    [Theory]
    [InlineData(48.85, 2.35, 1)]
    [InlineData(48.85, 2.35, 24)]
    [InlineData(48.85, 2.35, 48)]
    [InlineData(-90, -180, 1)]
    [InlineData(90, 180, 48)]
    public void Validate_WithValidParameters_ReturnsValid(double lat, double lon, int hours)
    {
        var result = _sut.Validate(new GetHourlyRequest(lat, lon, hours));

        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData(-91, 0, 24, nameof(GetHourlyRequest.Lat))]
    [InlineData(91, 0, 24, nameof(GetHourlyRequest.Lat))]
    [InlineData(0, -181, 24, nameof(GetHourlyRequest.Lon))]
    [InlineData(0, 181, 24, nameof(GetHourlyRequest.Lon))]
    [InlineData(48.85, 2.35, 0, nameof(GetHourlyRequest.Hours))]
    [InlineData(48.85, 2.35, 49, nameof(GetHourlyRequest.Hours))]
    public void Validate_WithInvalidParameters_ReturnsValidationError(
        double lat,
        double lon,
        int hours,
        string propertyName
    )
    {
        var result = _sut.Validate(new GetHourlyRequest(lat, lon, hours));

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, error => error.PropertyName == propertyName);
    }
}
