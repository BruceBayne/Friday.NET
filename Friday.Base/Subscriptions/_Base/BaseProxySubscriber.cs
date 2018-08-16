using System;
using System.Threading.Tasks;

namespace Friday.Base.Subscriptions._Base
{
	public abstract class BaseProxySubscriber<TCriteria, TNextArg> where TCriteria : IEquatable<TCriteria>
	{
		protected abstract Task<IDisposable> SubscribeInternal(TCriteria criteria,
			ReactiveActions<TNextArg> actions);

		protected Task<IDisposable> SubscribeInternal(TCriteria criteria,
			Action<TNextArg> onNextAction, Action<Exception> errorAction)
		{
			return SubscribeInternal(criteria, new ReactiveActions<TNextArg>(onNextAction, errorAction));
		}
	}
}