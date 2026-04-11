namespace WeatherApp.Api.Features.Weather.GetForecast;

internal sealed record GetForecastRequest(double Lat, double Lon, int Days = 7);
