using System;

namespace Friday.Base.ValueTypes
{
	[Serializable]
	public struct SubscriptionId : IEquatable<SubscriptionId>, IComparable<SubscriptionId>
	{
		public readonly uint Id;

		public override string ToString()
		{
			return $"{nameof(Id)}: {Id}";
		}

		public SubscriptionId(uint id)
		{
			Id = id;
		}

		public bool Equals(SubscriptionId other)
		{
			return Id == other.Id;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			return obj is SubscriptionId id && Equals(id);
		}

		public override int GetHashCode()
		{
			return (int)Id;
		}

		public int CompareTo(SubscriptionId other)
		{
			return Id.CompareTo(other.Id);
		}
	}
}