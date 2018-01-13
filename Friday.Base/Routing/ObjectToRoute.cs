using Friday.Base.Routing.Interfaces;

namespace Friday.Base.Routing
{
	public struct ObjectToRoute
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
	}
}