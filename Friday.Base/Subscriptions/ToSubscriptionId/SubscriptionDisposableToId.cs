using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Friday.Base.Subscriptions.Virtualizer;
using Friday.Base.Subscriptions._Base;
using Friday.Base.Subscriptions._Base.DataProvider;
using Friday.Base.ValueTypes;

namespace Friday.Base.Subscriptions.ToSubscriptionId
{
	/// <summary>
	/// Wraps provider IDisposable subscriptions into SubscriptionId's
	/// </summary>
	public class SubscriptionDisposableToId<TCriteria, TActionArg> : IUnsubscribe where TCriteria : IEquatable<TCriteria>
	{
		private readonly ISubscriber<TCriteria, TActionArg> provider;

		private readonly SubscriptionIdGenerator generator = new SubscriptionIdGenerator();

		private readonly ConcurrentDictionary<SubscriptionId, IDisposable> mapping =
			new ConcurrentDictionary<SubscriptionId, IDisposable>();


		public SubscriptionDisposableToId(ISubscriber<TCriteria, TActionArg> provider)
		{
			this.provider = provider;
		}


		public async Task<SubscriptionId> Subscribe(TCriteria criteria,
			Action<TActionArg> onNext, Action<Exception> onException)
		{
			var disposable = await provider.Subscribe(criteria, onNext, onException).ConfigureAwait(false);

			var id = generator.GetNewId();
			mapping.TryAdd(id, disposable);
			return id;
		}


		public Task<SubscriptionId> Subscribe(TCriteria criteria,
			ReactiveActions<TActionArg> actions)
		{
			return Subscribe(criteria, actions.NextAction, actions.ErrorAction);
		}

		public virtual bool Unsubscribe(SubscriptionId id)
		{
			if (!mapping.TryRemove(id, out var disposable))
				return false;
			disposable?.Dispose();
			return true;
		}
	}
}