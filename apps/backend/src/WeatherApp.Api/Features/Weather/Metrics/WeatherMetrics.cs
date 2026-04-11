using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace WeatherApp.Api.Features.Weather;

internal sealed class WeatherMetrics
{
    internal const string MeterName = "WeatherApp.Api";

    internal static class Endpoints
    {
        internal const string Current = "current";
        internal const string Forecast = "forecast";
        internal const string Hourly = "hourly";
    }

    internal static class InstrumentNames
    {
        internal const string Requests = "weather.requests";
        internal const string ProviderDuration = "weather.provider.duration";
        internal const string ProviderErrors = "weather.provider.errors";
    }

    internal static class TagKeys
    {
        internal const string Endpoint = "endpoint";
    }

    private readonly Counter<long> _requestsCounter;
    private readonly Histogram<double> _providerDuration;
    private readonly Counter<long> _providerErrors;

    public WeatherMetrics(IMeterFactory meterFactory)
    {
        var meter = meterFactory.Create(MeterName);

        _requestsCounter = meter.CreateCounter<long>(
            InstrumentNames.Requests,
            description: "Number of weather API requests"
        );

        _providerDuration = meter.CreateHistogram<double>(
            InstrumentNames.ProviderDuration,
            unit: "ms",
            description: "Duration of weather provider calls in milliseconds"
        );

        _providerErrors = meter.CreateCounter<long>(
            InstrumentNames.ProviderErrors,
            description: "Number of weather provider call failures"
        );
    }

    public void RequestCalled(string endpoint) =>
        _requestsCounter.Add(1, new TagList { { TagKeys.Endpoint, endpoint } });

    public void ProviderCallSucceeded(string endpoint, double durationMs) =>
        _providerDuration.Record(durationMs, new TagList { { TagKeys.Endpoint, endpoint } });

    public void ProviderCallFailed(string endpoint, double durationMs)
    {
        _providerDuration.Record(durationMs, new TagList { { TagKeys.Endpoint, endpoint } });
        _providerErrors.Add(1, new TagList { { TagKeys.Endpoint, endpoint } });
    }
}
