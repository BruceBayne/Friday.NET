using System;

namespace Friday.Telemetry.Measure.Measure.Gauge
{
    /// <summary>
    /// A Gauge is simply an action that returns the instantaneous measure of a value, where the value abitrarily increases and decreases, for example CPU usage.
    /// Gauges are ideal to use for measured values such as current memory usage, cpu usage, temperatures, disk space etc.
    /// </summary>
    [Serializable]
    public struct GaugeEvent
    {
        public readonly GaugeEventAction Action;
        public readonly string Name;

        public static GaugeEvent Mark(string name)
        {
            return new GaugeEvent(GaugeEventAction.Mark, name);
        }


        public GaugeEvent(GaugeEventAction action, string name)
        {
            Action = action;
            Name = name;
        }
    }
}