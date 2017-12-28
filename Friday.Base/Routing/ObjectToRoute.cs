using Friday.Base.Routing.Interfaces;

namespace Friday.Base.Routing
{
	public struct ObjectToRoute
	{
		public readonly IRoutingContext Context;
		public readonly object Payload;
		public readonly StaticRoutingTableRecord RouteRecord;

		public ObjectToRoute(IRoutingContext context, object payload, StaticRoutingTableRecord routeRecord)
		{
			Context = context;
			Payload = payload;
			RouteRecord = routeRecord;
		}
	}
}