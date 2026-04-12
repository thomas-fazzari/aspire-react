using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace WeatherApp.Api.Infrastructure.Metrics;

internal sealed class AppMetrics
{
    internal const string MeterName = "WeatherApp";

    internal static class InstrumentNames
    {
        internal const string Requests = "weatherapp.requests";
        internal const string OpenMeteoCalls = "weatherapp.openmeteo.calls";
        internal const string CacheHits = "weatherapp.cache.hits";
        internal const string CacheMisses = "weatherapp.cache.misses";
        internal const string UserRegistrations = "weatherapp.users.registrations";
    }

    internal static class TagKeys
    {
        internal const string Endpoint = "endpoint";
        internal const string Success = "success";
    }

    internal static class Endpoints
    {
        internal const string WeatherCurrent = "weather.current";
        internal const string WeatherForecast = "weather.forecast";
        internal const string WeatherHourly = "weather.hourly";
        internal const string Cities = "cities.list";
    }

    private readonly Counter<long> _requests;
    private readonly Counter<long> _openMeteoCalls;
    private readonly Counter<long> _cacheHits;
    private readonly Counter<long> _cacheMisses;
    private readonly Counter<long> _userRegistrations;

    public AppMetrics(IMeterFactory meterFactory)
    {
        var meter = meterFactory.Create(MeterName);
        _requests = meter.CreateCounter<long>(
            InstrumentNames.Requests,
            unit: "requests",
            description: "Request count by endpoint"
        );
        _openMeteoCalls = meter.CreateCounter<long>(
            InstrumentNames.OpenMeteoCalls,
            unit: "calls",
            description: "Open-Meteo API calls"
        );
        _cacheHits = meter.CreateCounter<long>(
            InstrumentNames.CacheHits,
            unit: "hits",
            description: "Cache hits"
        );
        _cacheMisses = meter.CreateCounter<long>(
            InstrumentNames.CacheMisses,
            unit: "misses",
            description: "Cache misses"
        );
        _userRegistrations = meter.CreateCounter<long>(
            InstrumentNames.UserRegistrations,
            unit: "registrations",
            description: "User registrations"
        );
    }

    public void Request(string endpoint) =>
        _requests.Add(1, new TagList { { TagKeys.Endpoint, endpoint } });

    public void OpenMeteoCall(bool success) =>
        _openMeteoCalls.Add(1, new TagList { { TagKeys.Success, success } });

    public void CacheHit() => _cacheHits.Add(1);

    public void CacheMiss() => _cacheMisses.Add(1);

    public void UserRegistered() => _userRegistrations.Add(1);
}
