namespace WeatherApp.Api.Features.Cities.GetCities;

internal sealed record CityResponse(string Name, double Lat, double Lon, string CountryCode);
