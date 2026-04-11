namespace WeatherApp.Api.Features.Weather.GetWeather;

internal sealed record WeatherResponse(double Lat, double Lon, CurrentConditions Current);

internal sealed record CurrentConditions(double Temperature, double WindSpeed, int WeatherCode);
