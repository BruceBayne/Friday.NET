using Friday.Base.ValueTypes;

namespace Friday.Base.Subscriptions.ToSubscriptionId
{
	public interface IUnsubscribe
	{
		bool Unsubscribe(SubscriptionId id);
	}
}