using System;

namespace Friday.Base.Routing.Interfaces
{
	public interface IObjectRouter
	{
		void RouteCall<T>(Action<T> callAction) where T : class;
		void RouteObject(object context, object objectForRoute);
		void RouteObject(object objectForRoute);
	}
}