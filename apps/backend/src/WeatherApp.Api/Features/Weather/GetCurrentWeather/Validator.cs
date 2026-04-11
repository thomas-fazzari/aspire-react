using FluentValidation;

namespace WeatherApp.Api.Features.Weather.GetCurrentWeather;

internal sealed class GetCurrentWeatherRequestValidator
    : AbstractValidator<GetCurrentWeatherRequest>
{
    public GetCurrentWeatherRequestValidator()
    {
        RuleFor(x => x.Lat).InclusiveBetween(-90, 90);
        RuleFor(x => x.Lon).InclusiveBetween(-180, 180);
    }
}
