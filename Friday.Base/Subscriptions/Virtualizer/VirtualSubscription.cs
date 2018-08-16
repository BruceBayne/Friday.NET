using System;
using Friday.Base.ValueTypes;

namespace Friday.Base.Subscriptions.Virtualizer
{
	/// <summary>
	/// Holds link SubscriptionID to appropriate IDisposable
	/// </summary>
	public sealed class VirtualSubscription : IDisposable
	{
		public readonly SubscriptionId SubscriptionId;

		public readonly IDisposable Disposable;
		public int Counter { get; private set; }

		public VirtualSubscription(SubscriptionId subscriptionId, IDisposable disposable)
		{
			SubscriptionId = subscriptionId;
			Disposable = disposable;
			Counter = 1;
		}

		public void IncreaseCounter()
		{
			Counter++;
		}


		public bool NoMoreSubscribers()
		{
			return Counter <= 0;
		}

		public void DecreaseCounter()
		{
			Counter--;
			if (Counter <= 0)
				Dispose();
		}

		public override string ToString()
		{
			return $"{nameof(SubscriptionId)}: {SubscriptionId}, {nameof(Counter)}: {Counter}";
		}

		public void Dispose()
		{
			Disposable.Dispose();
			Counter = 0;
		}

		public bool Has(SubscriptionId id)
		{
			return id.Equals(SubscriptionId);
		}
	}
}