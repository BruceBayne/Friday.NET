using System;

namespace Friday.Base.Routing.Interfaces
{
	public interface IObjectRouter
	{
		void RouteCall<T>(Action<T> callAction) where T : class;
		void RouteObject(IRoutingContext context, object routedObject);


	}
}