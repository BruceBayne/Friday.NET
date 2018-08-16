using System;
using System.Threading.Tasks;
using Friday.Base.Common;
using Friday.Base.Subscriptions.ToSubscriptionId;
using Friday.Base.Subscriptions._Base;
using Friday.Base.Subscriptions._Base.DataProvider;

namespace Friday.Base.Subscriptions.ToDisposable
{
	/// <summary>
	/// Converts SubscriptionID to disposable
	/// </summary>
	public sealed class SubscriptionIdToDisposable<TCriteria, TActionArg> : ISubscriber<TCriteria, TActionArg> where TCriteria : IEquatable<TCriteria>
	{
		private readonly SubscriptionDisposableToId<TCriteria, TActionArg> proxy;

		public SubscriptionIdToDisposable(SubscriptionDisposableToId<TCriteria, TActionArg> proxy)
		{

			this.proxy = proxy;
		}

		public async Task<IDisposable> Subscribe(TCriteria criteria,
			Action<TActionArg> onNext, Action<Exception> onException)
		{
			var id = await proxy.Subscribe(criteria,
				new ReactiveActions<TActionArg>(onNext, onException)).ConfigureAwait(false);
			return new FridayDisposable(() => { proxy.Unsubscribe(id); });
		}
	}
}