using FluentValidation;

namespace WeatherApp.Api.Features.Weather.GetHourly;

internal sealed class GetHourlyRequestValidator : AbstractValidator<GetHourlyRequest>
{
    public GetHourlyRequestValidator()
    {
        RuleFor(x => x.Lat).InclusiveBetween(-90, 90);
        RuleFor(x => x.Lon).InclusiveBetween(-180, 180);
        RuleFor(x => x.Hours).InclusiveBetween(1, 48);
    }
}
