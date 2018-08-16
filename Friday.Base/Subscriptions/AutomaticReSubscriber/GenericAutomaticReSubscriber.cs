using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Friday.Base.Subscriptions._Base;
using Friday.Base.Subscriptions._Base.DataProvider;

namespace Friday.Base.Subscriptions.AutomaticReSubscriber
{
	/// <summary>
	/// Provide automatic ReSubscription after underlying provider is connected
	/// </summary>
	public abstract class GenericAutomaticReSubscriber<TCriteria, TActionArgument> : IEventsWithSubscribe<TCriteria, TActionArgument> where TCriteria : IEquatable<TCriteria>
	{
		private readonly IEventsWithSubscribe<TCriteria, TActionArgument> wrappedProvider;
		private readonly List<ResubscribeCacheElement<TCriteria, TActionArgument>> cache = new List<ResubscribeCacheElement<TCriteria, TActionArgument>>();

		public event EventHandler<Exception> OnException;


		protected GenericAutomaticReSubscriber(IEventsWithSubscribe<TCriteria, TActionArgument> wrappedProvider)
		{
			this.wrappedProvider = wrappedProvider;
			wrappedProvider.OnConnected += ProviderConnected;
			wrappedProvider.OnDisconnected += ProviderDisconnected;
			wrappedProvider.OnException += WrappedProviderOnOnException;
		}

		private void WrappedProviderOnOnException(object sender, Exception exception)
		{
			DoOnException(exception);
		}

		public Task<IDisposable> Subscribe(TCriteria criteria,
			Action<TActionArgument> onNextAction,
			Action<Exception> onException)
		{
			return Subscribe(criteria, new ReactiveActions<TActionArgument>(onNextAction, onException));
		}

		protected virtual void ProviderConnected(object sender, EventArgs eventArgs)
		{
			CleanupDisposedCacheElements();
			ResubscribeOnCacheElements();
		}


		public async Task<IDisposable> Subscribe(TCriteria criteria,
			ReactiveActions<TActionArgument> actions)
		{
			var cacheElement = new ResubscribeCacheElement<TCriteria, TActionArgument>(criteria, actions);
			cache.Add(cacheElement);

			await TrySubscribeOnCacheElement(cacheElement).ConfigureAwait(false);

			return cacheElement;
		}

		protected virtual Task OnReSubscribed(TCriteria criteria,
			Action<TActionArgument> onNextAction,
			Action<Exception> onException)
		{
			return Task.CompletedTask;
		}

		protected async Task<bool> TrySubscribeOnCacheElement(ResubscribeCacheElement<TCriteria, TActionArgument> cacheElement, bool isReSubscribe = false)
		{
			try
			{
				var providerSubscription = await wrappedProvider.Subscribe(cacheElement.Criteria,
					cacheElement.Actions.NextAction,
					cacheElement.Actions.ErrorAction).ConfigureAwait(false);

				cacheElement.MakeSubscriptionAlive(providerSubscription);

				if (isReSubscribe)
					await OnReSubscribed(cacheElement.Criteria, cacheElement.Actions.NextAction, cacheElement.Actions.ErrorAction).ConfigureAwait(false);

				return true;
			}
			catch (Exception e)
			{
				cacheElement.MakeSubscriptionRotten();
				DoOnException(e);
				return false;
			}
		}

		protected virtual void ProviderDisconnected(object sender, EventArgs eventArgs)
		{
			try
			{
				CleanupDisposedCacheElements();
				foreach (var cacheElement in cache)
					cacheElement.MakeSubscriptionRotten();
			}
			catch (Exception e)
			{
				DoOnException(e);
			}
		}

		private void CleanupDisposedCacheElements()
		{
			cache.RemoveAll(x => x.CurrentStatus == LiveSubscriptionStatus.Disposed);
		}


		private void ResubscribeOnCacheElements()
		{
			Task.Run(async () =>
			{
				try
				{
					foreach (var cacheElement in cache)
					{
						if (!await TrySubscribeOnCacheElement(cacheElement, isReSubscribe: true).ConfigureAwait(false))
							break;
					}
				}
				catch (Exception e)
				{
					DoOnException(e);
				}
			});
		}




		public void Stop()
		{
			wrappedProvider.OnConnected -= ProviderConnected;
			wrappedProvider.OnDisconnected -= ProviderDisconnected;
			cache.Clear();
		}


		private void DoOnException(Exception exception)
		{
			OnException?.Invoke(this, exception);
		}

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
	}
}