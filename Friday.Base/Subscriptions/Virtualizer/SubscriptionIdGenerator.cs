using Friday.Base.ValueTypes;

namespace Friday.Base.Subscriptions.Virtualizer
{
	public sealed class SubscriptionIdGenerator
	{
		private uint nextAvailableId;

		public SubscriptionId GetNewId()
		{
			lock (this)
			{
				nextAvailableId++;
				return new SubscriptionId(nextAvailableId);
			}
		}
	}
}