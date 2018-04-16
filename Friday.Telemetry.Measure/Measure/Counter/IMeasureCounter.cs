using System;

namespace Friday.Telemetry.Measure.Measure.Counter
{
    public interface ITelemetryCounter
    {
        event EventHandler<CounterEvent> CounterChanged;
    }
}