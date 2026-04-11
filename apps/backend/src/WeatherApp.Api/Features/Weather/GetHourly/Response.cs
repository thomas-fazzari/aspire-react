namespace WeatherApp.Api.Features.Weather.GetHourly;

internal sealed record HourlyResponse(double Lat, double Lon, HourlyConditionsDto[] Hours);

internal sealed record HourlyConditionsDto(
    string Time,
    double Temperature,
    int WeatherCode,
    double PrecipitationProbability,
    double WindSpeed
);
