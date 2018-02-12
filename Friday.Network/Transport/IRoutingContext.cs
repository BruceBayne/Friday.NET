using System;
using Friday.Base;
using Friday.Base.Routing.Interfaces;

namespace Friday.Network.Transport
{
	public interface IRoutingContext<TServerMessageBase> : IStartableMarker, IObjectRouter
	{
		
		event EventHandler<TServerMessageBase> OnMessageAvailable;
		void Dispose();
	}
}