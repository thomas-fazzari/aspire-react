using System.Diagnostics.Metrics;
using Microsoft.Extensions.Diagnostics.Metrics.Testing;
using WeatherApp.Api.Infrastructure.Metrics;

namespace WeatherApp.UnitTests.Infrastructure.Metrics;

public sealed class AppMetricsTests
{
    [Fact]
    public void Request_IncrementsRequestsCounter()
    {
        using var meter = new Meter(AppMetrics.MeterName);
        using var collector = new MetricCollector<long>(meter, AppMetrics.InstrumentNames.Requests);

        var metrics = new AppMetrics(new MeterFactoryFromMeter(meter));
        metrics.Request(AppMetrics.Endpoints.WeatherCurrent);

        var measurements = collector.GetMeasurementSnapshot();
        Assert.Single(measurements);
        Assert.Equal(1, measurements[0].Value);
    }

    [Fact]
    public void OpenMeteoCall_IncrementsCallsCounter()
    {
        using var meter = new Meter(AppMetrics.MeterName);
        using var collector = new MetricCollector<long>(
            meter,
            AppMetrics.InstrumentNames.OpenMeteoCalls
        );

        var metrics = new AppMetrics(new MeterFactoryFromMeter(meter));
        metrics.OpenMeteoCall(true);
        metrics.OpenMeteoCall(false);

        var measurements = collector.GetMeasurementSnapshot();
        Assert.Equal(2, measurements.Count);
    }

    [Fact]
    public void CacheHit_IncrementsCacheHitsCounter()
    {
        using var meter = new Meter(AppMetrics.MeterName);
        using var collector = new MetricCollector<long>(
            meter,
            AppMetrics.InstrumentNames.CacheHits
        );

        var metrics = new AppMetrics(new MeterFactoryFromMeter(meter));
        metrics.CacheHit();

        var measurements = collector.GetMeasurementSnapshot();
        Assert.Single(measurements);
        Assert.Equal(1, measurements[0].Value);
    }

    [Fact]
    public void CacheMiss_IncrementsCacheMissesCounter()
    {
        using var meter = new Meter(AppMetrics.MeterName);
        using var collector = new MetricCollector<long>(
            meter,
            AppMetrics.InstrumentNames.CacheMisses
        );

        var metrics = new AppMetrics(new MeterFactoryFromMeter(meter));
        metrics.CacheMiss();

        var measurements = collector.GetMeasurementSnapshot();
        Assert.Single(measurements);
        Assert.Equal(1, measurements[0].Value);
    }

    [Fact]
    public void UserRegistered_IncrementsRegistrationsCounter()
    {
        using var meter = new Meter(AppMetrics.MeterName);
        using var collector = new MetricCollector<long>(
            meter,
            AppMetrics.InstrumentNames.UserRegistrations
        );

        var metrics = new AppMetrics(new MeterFactoryFromMeter(meter));
        metrics.UserRegistered();

        var measurements = collector.GetMeasurementSnapshot();
        Assert.Single(measurements);
        Assert.Equal(1, measurements[0].Value);
    }

    private sealed class MeterFactoryFromMeter(Meter meter) : IMeterFactory
    {
        public Meter Create(MeterOptions options) => meter;

        public void Dispose() { }
    }
}
