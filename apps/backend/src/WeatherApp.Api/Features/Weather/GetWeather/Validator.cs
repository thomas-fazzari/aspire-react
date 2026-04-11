using FluentValidation;

namespace WeatherApp.Api.Features.Weather.GetWeather;

internal sealed class GetWeatherRequestValidator : AbstractValidator<GetWeatherRequest>
{
    public GetWeatherRequestValidator()
    {
        RuleFor(x => x.Lat).InclusiveBetween(-90, 90);
        RuleFor(x => x.Lon).InclusiveBetween(-180, 180);
    }
}
