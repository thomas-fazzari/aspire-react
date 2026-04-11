namespace WeatherApp.Api.Features.Weather.GetHourly;

internal sealed record HourlyResponse(double Lat, double Lon, HourlyEntryDto[] Hours);

internal sealed record HourlyEntryDto(
    string Time,
    double Temperature,
    int WeatherCode,
    double PrecipitationProbability,
    double WindSpeed
);
