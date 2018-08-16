using System;
using System.Reflection;

namespace Friday.Base.Routing
{
	[Serializable]
	public struct StaticRoutingTableRecord : IEquatable<StaticRoutingTableRecord>
	{
		public readonly MethodInfo SelectedMethod;
		public readonly object Processor;

		public StaticRoutingTableRecord(object processor, MethodInfo selectedMethod)
		{
			Processor = processor;
			SelectedMethod = selectedMethod;
		}

		public bool Equals(StaticRoutingTableRecord other)
		{
			return Equals(SelectedMethod, other.SelectedMethod) && Equals(Processor, other.Processor);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			return obj is StaticRoutingTableRecord && Equals((StaticRoutingTableRecord)obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return ((SelectedMethod != null ? SelectedMethod.GetHashCode() : 0) * 397) ^ (Processor != null ? Processor.GetHashCode() : 0);
			}
		}
	}
}