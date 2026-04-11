namespace WeatherApp.Api.Features.Weather.GetHourly;

internal sealed record GetHourlyRequest(double Lat, double Lon, int Hours = 24);
