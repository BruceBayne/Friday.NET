using System;

namespace Friday.Telemetry.Metrics.Measure.Counter

{
    /// <summary>
    /// Counters are one of the simpliest metrics types supported and allow us to track how many times something has happened. 
    /// They are an atomic 64-bit integer which can be incremented or decremented.
    /// Counters are ideal for counting occurances, events or errors for example.
    /// They also provide the ability to track a count and percentage of each item with a set.
    /// </summary>
    [Serializable]
    public struct CounterEvent
    {
        public readonly string CounterName;
        public readonly CounterEventAction CounterChange;

        public readonly long Value;

        public override string ToString()
        {
            return $"{nameof(CounterName)}: {CounterName}, {nameof(CounterChange)}: {CounterChange}";
        }

        public static CounterEvent Decreased(string name, long value = 1)
        {
            return new CounterEvent(CounterEventAction.Decreased, name, value);
        }


        public static CounterEvent Increased(string name, long value = 1)
        {
            return new CounterEvent(CounterEventAction.Increased, name, value);
        }

        public CounterEvent(CounterEventAction counterChange, string counterName, long Value)
        {
            CounterChange = counterChange;
            CounterName = counterName;
            this.Value = Value;
        }
    }
}