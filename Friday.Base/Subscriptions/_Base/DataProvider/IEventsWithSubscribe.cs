using System;

namespace Friday.Base.Subscriptions._Base.DataProvider
{
	public interface IEventsWithSubscribe<in TCriteria, out TActionArgument> : IDataProviderEvents,
		ISubscriber<TCriteria, TActionArgument> where TCriteria : IEquatable<TCriteria>
	{
	}
}