using System;

namespace Friday.Telemetry.Measure.Measure.Gauge
{
  public  interface ITelemetryGauge
    {
        event EventHandler<GaugeEvent> MetricMark;
    }

}
