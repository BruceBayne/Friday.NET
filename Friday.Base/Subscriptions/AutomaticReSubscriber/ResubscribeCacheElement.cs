using System;
using Friday.Base.Subscriptions._Base;

namespace Friday.Base.Subscriptions.AutomaticReSubscriber
{
	public class ResubscribeCacheElement<TFilterCriteria, TActionArg> : IDisposable
	{
		public TFilterCriteria Criteria { get; }
		public ReactiveActions<TActionArg> Actions { get; }

		public override string ToString()
		{
			return $"{nameof(CurrentStatus)}: {CurrentStatus}, {nameof(Criteria)}: {Criteria}";
		}

		public IDisposable ProviderSubscription { get; private set; }
		public LiveSubscriptionStatus CurrentStatus = LiveSubscriptionStatus.Invalid;


		public ResubscribeCacheElement(TFilterCriteria criteria, ReactiveActions<TActionArg> actions)
		{
			ProviderSubscription = null;
			Criteria = criteria;
			Actions = actions;
			CurrentStatus = LiveSubscriptionStatus.Rotten;
		}

		public ResubscribeCacheElement(TFilterCriteria criteria, ReactiveActions<TActionArg> actions,
			IDisposable providerSubscription)
		{
			ProviderSubscription = providerSubscription;
			Criteria = criteria;
			Actions = actions;
		}

		public void Dispose()
		{
			CurrentStatus = LiveSubscriptionStatus.Disposed;
			ProviderSubscription?.Dispose();
		}

		public void MakeSubscriptionRotten()
		{
			if (CurrentStatus == LiveSubscriptionStatus.Disposed)
				return;

			CurrentStatus = LiveSubscriptionStatus.Rotten;
			ProviderSubscription = null;
		}

		public void MakeSubscriptionAlive(IDisposable providerSubscription)
		{
			if (CurrentStatus == LiveSubscriptionStatus.Disposed)
				return;

			CurrentStatus = LiveSubscriptionStatus.Alive;
			ProviderSubscription?.Dispose();
			ProviderSubscription = providerSubscription;
		}
	}
}