using System;
using Friday.Base.Routing.Interfaces;

namespace Friday.Base.Routing
{
	[Serializable]
	public struct ObjectToRoute : IEquatable<ObjectToRoute>
	{
		public readonly object Context;
		public readonly object Payload;
		public readonly StaticRoutingTableRecord RouteRecord;

		public ObjectToRoute(object context, object payload, StaticRoutingTableRecord routeRecord)
		{
			Context = context;
			Payload = payload;
			RouteRecord = routeRecord;
		}

		public bool Equals(ObjectToRoute other)
		{
			return Equals(Context, other.Context) && Equals(Payload, other.Payload) && RouteRecord.Equals(other.RouteRecord);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			return obj is ObjectToRoute && Equals((ObjectToRoute)obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				var hashCode = (Context != null ? Context.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (Payload != null ? Payload.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ RouteRecord.GetHashCode();
				return hashCode;
			}
		}
	}
}