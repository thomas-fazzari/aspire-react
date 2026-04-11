using System.Diagnostics.Metrics;
using Microsoft.Extensions.Diagnostics.Metrics.Testing;
using WeatherApp.Api.Features.Weather;

namespace WeatherApp.UnitTests.Weather.Metrics;

public sealed class WeatherMetricsTests
{
    [Fact]
    public void RequestCalled_IncrementsRequestCounter()
    {
        using var meter = new Meter(WeatherMetrics.MeterName);
        using var collector = new MetricCollector<long>(
            meter,
            WeatherMetrics.InstrumentNames.Requests
        );

        var metrics = new WeatherMetrics(new MeterFactoryFromMeter(meter));
        metrics.RequestCalled(WeatherMetrics.Endpoints.Current);

        var measurements = collector.GetMeasurementSnapshot();
        Assert.Single(measurements);
        Assert.Equal(1, measurements[0].Value);
    }

    [Fact]
    public void ProviderCallSucceeded_RecordsDuration()
    {
        using var meter = new Meter(WeatherMetrics.MeterName);
        using var collector = new MetricCollector<double>(
            meter,
            WeatherMetrics.InstrumentNames.ProviderDuration
        );

        var metrics = new WeatherMetrics(new MeterFactoryFromMeter(meter));
        metrics.ProviderCallSucceeded(WeatherMetrics.Endpoints.Forecast, 150.5);

        var measurements = collector.GetMeasurementSnapshot();
        Assert.Single(measurements);
        Assert.Equal(150.5, measurements[0].Value);
    }

    [Fact]
    public void ProviderCallFailed_RecordsDurationAndIncrementsErrorCounter()
    {
        using var meter = new Meter(WeatherMetrics.MeterName);
        using var durationCollector = new MetricCollector<double>(
            meter,
            WeatherMetrics.InstrumentNames.ProviderDuration
        );
        using var errorCollector = new MetricCollector<long>(
            meter,
            WeatherMetrics.InstrumentNames.ProviderErrors
        );

        var metrics = new WeatherMetrics(new MeterFactoryFromMeter(meter));
        metrics.ProviderCallFailed(WeatherMetrics.Endpoints.Hourly, 200.0);

        var durationMeasurements = durationCollector.GetMeasurementSnapshot();
        Assert.Single(durationMeasurements);
        Assert.Equal(200.0, durationMeasurements[0].Value);

        var errorMeasurements = errorCollector.GetMeasurementSnapshot();
        Assert.Single(errorMeasurements);
        Assert.Equal(1, errorMeasurements[0].Value);
    }

    [Fact]
    public void RequestCalled_WithDifferentEndpoints_TracksSeparately()
    {
        using var meter = new Meter(WeatherMetrics.MeterName);
        using var collector = new MetricCollector<long>(
            meter,
            WeatherMetrics.InstrumentNames.Requests
        );

        var metrics = new WeatherMetrics(new MeterFactoryFromMeter(meter));
        metrics.RequestCalled(WeatherMetrics.Endpoints.Current);
        metrics.RequestCalled(WeatherMetrics.Endpoints.Current);
        metrics.RequestCalled(WeatherMetrics.Endpoints.Forecast);

        var measurements = collector.GetMeasurementSnapshot();
        Assert.Equal(3, measurements.Count);
    }

    private sealed class MeterFactoryFromMeter(Meter meter) : IMeterFactory
    {
        public Meter Create(MeterOptions options) => meter;

        public void Dispose() { }
    }
}
