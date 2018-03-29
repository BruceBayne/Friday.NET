using System;
using Friday.Base;
using Friday.Base.Routing.Interfaces;

namespace Friday.Network.Transport
{
	public interface IRoutingContext<TServerMessageBase> : ICompleteObjectRouter
	{

		event EventHandler<TServerMessageBase> OnMessageAvailable;

		void Start();
		void Dispose();
	}
}