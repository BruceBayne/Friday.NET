using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Friday.Base.Common;
using Friday.Base.Subscriptions.ToDisposable;
using Friday.Base.Subscriptions._Base;
using Friday.Base.Subscriptions._Base.DataProvider;

namespace Friday.Base.Subscriptions.RemoveDuplicates
{
	/// <summary>
	/// Wrap provider for avoid duplicate subscriptions on same criteria
	/// Thread safe
	/// </summary>
	public abstract class GenericRemoveDuplicateSubscriptions<TCriteria, TNextArg> : BaseProxySubscriber<TCriteria, TNextArg>, ISubscriber<TCriteria, TNextArg> where TCriteria : IEquatable<TCriteria>
	{
		private readonly ConcurrentDictionary<TCriteria, GenericSubscribersCriteriaBlock<TCriteria, TNextArg>> blocks =
			new ConcurrentDictionary<TCriteria, GenericSubscribersCriteriaBlock<TCriteria, TNextArg>>();


		public Task<IDisposable> Subscribe(TCriteria criteria, ReactiveActions<TNextArg> actions)
		{
			return Subscribe(criteria, actions.NextAction, actions.ErrorAction);
		}

		public async Task<IDisposable> Subscribe(TCriteria criteria, Action<TNextArg> onNextAction, Action<Exception> errorAction)
		{
			if (blocks.TryGetValue(criteria, out var block))
				return block.AddSubscriber(onNextAction, errorAction);


			var newBlock = blocks.GetOrAdd(criteria, new GenericSubscribersCriteriaBlock<TCriteria, TNextArg>(criteria));


			var realSubscription = await SubscribeInternal(criteria, newBlock.ProcessNext, newBlock.ProcessError).ConfigureAwait(false);

			var subscription = new FridayDisposable(() =>
			{
				blocks.TryRemove(criteria, out _);
				realSubscription.Dispose();
			});


			newBlock.SetRealSubscription(subscription);
			return newBlock.AddSubscriber(onNextAction, errorAction);
		}
	}
}