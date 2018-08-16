using System;

namespace Friday.Base.Subscriptions._Base.DataProvider
{
	public interface IDataProviderEvents : IOnExceptionEvent
	{
		event EventHandler OnConnected;
		event EventHandler OnDisconnected;
	}
}