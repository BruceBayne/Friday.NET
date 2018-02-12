using System;
using Friday.Base.Routing.Interfaces;

namespace Friday.Network.Transport
{
	public interface IRoutingContext<TServerMessageBase> : IObjectRouter
	{
		void Initialize();
		void RouteMessage(object context, TServerMessageBase message);
		event EventHandler<TServerMessageBase> OnMessageAvailable;
		void Dispose();
	}
}