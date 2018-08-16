using System;
using System.Collections.Generic;
using System.Linq;
using Friday.Base.Common;
using Friday.Base.Subscriptions.ToDisposable;
using Friday.Base.Subscriptions._Base;

namespace Friday.Base.Subscriptions.RemoveDuplicates
{
	/// <summary>
	/// Contains single Criteria with many subscribers(customers)
	/// Contains single REAL subscription (blockDisposable)
	/// </summary>
	public class GenericSubscribersCriteriaBlock<TCriteria, TNextArg>
	{
		public readonly TCriteria Criteria;
		private IDisposable blockDisposable;

		private List<SingleSubscriberInfo<TNextArg>> subscribers = new List<SingleSubscriberInfo<TNextArg>>();

		public GenericSubscribersCriteriaBlock(TCriteria criteria)
		{
			Criteria = criteria;
		}

		public void SetRealSubscription(IDisposable realSubscription)
		{
			blockDisposable = realSubscription;
		}

		public IDisposable AddSubscriber(Action<TNextArg> onNextAction, Action<Exception> errorAction)
		{
			var ssi = new SingleSubscriberInfo<TNextArg>(
				new ReactiveActions<TNextArg>(onNextAction, errorAction));

			var d = new FridayDisposable(() => { UnsubscribeSingleClient(ssi); });

			lock (this)
			{
				subscribers.Add(ssi);
			}

			return d;
		}

		private void UnsubscribeSingleClient(SingleSubscriberInfo<TNextArg> ssi)
		{
			bool blockIsAlive;

			lock (this)
			{
				subscribers.Remove(ssi);
				blockIsAlive = subscribers.Any();
			}

			if (!blockIsAlive)
				DisposeThisBlock();
		}


		public void ProcessNext(TNextArg nextArg)
		{
			var callList = GetSafeCopy();
			callList.ForEach(x => x.CallOnNext(nextArg));
		}

		private List<SingleSubscriberInfo<TNextArg>> GetSafeCopy()
		{
			List<SingleSubscriberInfo<TNextArg>> callList;
			lock (subscribers)
				callList = subscribers.ToList();
			return callList;
		}

		public void ProcessError(Exception exception)
		{
			var callList = GetSafeCopy();
			callList.ForEach(x => x.CallOnError(exception));
		}

		private void DisposeThisBlock()
		{
			lock (subscribers)
			{
				blockDisposable?.Dispose();
				blockDisposable = null;
				subscribers = new List<SingleSubscriberInfo<TNextArg>>();
			}
		}
	}
}