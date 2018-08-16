using System;
using System.Threading.Tasks;
using Friday.Base.Subscriptions._Base.DataProvider;

namespace Friday.Base.Subscriptions._Base
{
	public static class SubscriptionProviderExtensions
	{
		public static Task<IDisposable> Subscribe<TFilterCriteria, TActionArg>(this ISubscriber<TFilterCriteria, TActionArg> subscriber,
			TFilterCriteria criteria, ReactiveActions<TActionArg> actions) where TFilterCriteria : IEquatable<TFilterCriteria>
		{
			return subscriber.Subscribe(criteria, actions.NextAction, actions.ErrorAction);
		}
	}
}