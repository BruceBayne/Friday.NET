using System;
using System.Threading.Tasks;
using Friday.Base.Subscriptions._Base.DataProvider;

namespace Friday.Base.Subscriptions.AlwaysOn
{

	/// <summary>
	/// Tries to keep underlying provider always connected
	/// </summary>
	public abstract class GenericAlwaysOn : IProviderBasics
	{
		private readonly IProviderBasics wrappedProvider;
		public event EventHandler<Exception> OnException;
		public event EventHandler OnReconnect;
		public readonly TimeSpan ReconnectDelay = TimeSpan.FromSeconds(3);
		public event EventHandler OnDisconnected;
		public bool IsReconnecting { get; private set; }


		protected virtual Task ReconnectProvider()
		{
			IsReconnecting = true;
			DoOnReconnect();
			return wrappedProvider.Connect();
		}

		protected virtual async void ProviderDisconnected(object sender, EventArgs eventArgs)
		{
			IsReconnecting = false;
			OnDisconnected?.Invoke(this, EventArgs.Empty);
			await Task.Delay(ReconnectDelay).ConfigureAwait(false);
			await ConnectSafely().ConfigureAwait(false);
		}


		public Task Connect()
		{
			wrappedProvider.OnDisconnected += ProviderDisconnected;
			return ConnectSafely();
		}

		private async Task ConnectSafely()
		{
			try
			{
				await ReconnectProvider().ConfigureAwait(false);
				IsReconnecting = false;
			}
			catch (Exception e)
			{
				ProviderDisconnected(this, EventArgs.Empty);
				DoOnException(e);
			}
		}

		public void Disconnect()
		{
			wrappedProvider.Disconnect();
			wrappedProvider.OnDisconnected -= ProviderDisconnected;
			IsReconnecting = false;
		}

		protected GenericAlwaysOn(IProviderBasics wrappedProvider)
		{
			this.wrappedProvider = wrappedProvider;
		}


		protected virtual void DoOnException(Exception e)
		{
			OnException?.Invoke(this, e);
		}

		protected virtual void DoOnReconnect()
		{
			OnReconnect?.Invoke(this, EventArgs.Empty);
		}

		public event EventHandler OnConnected
		{
			add => wrappedProvider.OnConnected += value;
			remove => wrappedProvider.OnConnected -= value;
		}
	}
}