using System;

namespace Friday.Telemetry.Metrics.Measure.Gauge
{
  public  interface IMeasureGauge
    {
        event EventHandler<GaugeEvent> MetricMark;
    }

}
