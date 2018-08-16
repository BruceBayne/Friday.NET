using System;
using Friday.Base.Subscriptions._Base.DataProvider;

namespace Friday.Base.Subscriptions._Base
{
	public abstract class ProxyBasics : IDataProviderEvents
	{
		private readonly IProviderBasics wrappedProvider;

		public event EventHandler OnConnected
		{
			add => wrappedProvider.OnConnected += value;
			remove => wrappedProvider.OnConnected -= value;
		}


		public event EventHandler OnDisconnected
		{
			add => wrappedProvider.OnDisconnected += value;
			remove => wrappedProvider.OnDisconnected -= value;
		}

		public event EventHandler<Exception> OnException
		{
			add => wrappedProvider.OnException += value;
			remove => wrappedProvider.OnException -= value;
		}


		protected ProxyBasics(IProviderBasics wrappedProvider)
		{
			this.wrappedProvider = wrappedProvider;
		}
	}
}