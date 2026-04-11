using FluentValidation;

namespace WeatherApp.Api.Features.Weather.GetForecast;

internal sealed class GetForecastRequestValidator : AbstractValidator<GetForecastRequest>
{
    public GetForecastRequestValidator()
    {
        RuleFor(x => x.Lat).InclusiveBetween(-90, 90);
        RuleFor(x => x.Lon).InclusiveBetween(-180, 180);
        RuleFor(x => x.Days).InclusiveBetween(1, 16);
    }
}
