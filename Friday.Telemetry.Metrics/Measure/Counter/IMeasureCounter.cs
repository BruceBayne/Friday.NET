using System;

namespace Friday.Telemetry.Metrics.Measure.Counter
{
    public interface IMeasureCounter
    {
        event EventHandler<CounterEvent> CounterChanged;
    }
}