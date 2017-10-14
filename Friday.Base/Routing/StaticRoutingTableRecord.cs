using System.Reflection;

namespace Friday.Base.Routing
{
    public struct StaticRoutingTableRecord
    {
        public readonly MethodInfo SelectedMethod;
        public readonly object Processor;

        public StaticRoutingTableRecord(object processor, MethodInfo selectedMethod)
        {
            Processor = processor;
            SelectedMethod = selectedMethod;
        }
    }
}