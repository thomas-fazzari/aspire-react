namespace WeatherApp.Api.Domain.Cities;

internal sealed class City
{
    private City() { }

    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public double Lat { get; private set; }
    public double Lon { get; private set; }
    public string CountryCode { get; private set; } = string.Empty;

    public static City Create(string name, double lat, double lon, string countryCode) =>
        new()
        {
            Id = Guid.NewGuid(),
            Name = name,
            Lat = lat,
            Lon = lon,
            CountryCode = countryCode,
        };
}
