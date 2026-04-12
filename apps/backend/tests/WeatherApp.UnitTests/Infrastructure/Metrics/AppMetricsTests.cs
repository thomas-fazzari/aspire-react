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
    public void OpenMeteoCallSucceeded_IncrementsCounterAndRecordsDuration()
    {
        using var meter = new Meter(AppMetrics.MeterName);
        using var counterCollector = new MetricCollector<long>(
            meter,
            AppMetrics.InstrumentNames.OpenMeteoCalls
        );
        using var durationCollector = new MetricCollector<double>(
            meter,
            AppMetrics.InstrumentNames.OpenMeteoCallDuration
        );

        var metrics = new AppMetrics(new MeterFactoryFromMeter(meter));
        metrics.OpenMeteoCallSucceeded(TimeSpan.FromMilliseconds(150));

        var counterMeasurements = counterCollector.GetMeasurementSnapshot();
        Assert.Single(counterMeasurements);
        Assert.Equal(1, counterMeasurements[0].Value);

        var durationMeasurements = durationCollector.GetMeasurementSnapshot();
        Assert.Single(durationMeasurements);
        Assert.Equal(150, durationMeasurements[0].Value);
    }

    [Fact]
    public void OpenMeteoCallFailed_IncrementsCounterAndRecordsDuration()
    {
        using var meter = new Meter(AppMetrics.MeterName);
        using var counterCollector = new MetricCollector<long>(
            meter,
            AppMetrics.InstrumentNames.OpenMeteoCalls
        );
        using var durationCollector = new MetricCollector<double>(
            meter,
            AppMetrics.InstrumentNames.OpenMeteoCallDuration
        );

        var metrics = new AppMetrics(new MeterFactoryFromMeter(meter));
        metrics.OpenMeteoCallFailed(TimeSpan.FromMilliseconds(200));

        var counterMeasurements = counterCollector.GetMeasurementSnapshot();
        Assert.Single(counterMeasurements);
        Assert.Equal(1, counterMeasurements[0].Value);

        var durationMeasurements = durationCollector.GetMeasurementSnapshot();
        Assert.Single(durationMeasurements);
        Assert.Equal(200, durationMeasurements[0].Value);
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
