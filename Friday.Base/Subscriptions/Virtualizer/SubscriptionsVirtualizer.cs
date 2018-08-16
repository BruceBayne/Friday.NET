using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Friday.Base.Subscriptions._Base;
using Friday.Base.ValueTypes;

namespace Friday.Base.Subscriptions.Virtualizer
{
	/// <summary>
	/// По одной критерии отдаст туже самую АЙДИ на подписку
	/// Поддерживает счётчик подписок для одной критерии
	/// Returns same ID for same subscription
	/// </summary>
	/// <typeparam name="TCriteria"></typeparam>
	/// <typeparam name="TNextArgument"></typeparam>
	public abstract class SubscriptionsVirtualizer<TCriteria, TNextArgument> : BaseProxySubscriber<TCriteria, TNextArgument>, IDisposable where TCriteria : IEquatable<TCriteria>
	{
		private readonly Dictionary<TCriteria, VirtualSubscription> subscriptions =
			new Dictionary<TCriteria, VirtualSubscription>();

		private readonly SemaphoreSlim slimSemaphore = new SemaphoreSlim(1);

		private readonly SubscriptionIdGenerator generator = new SubscriptionIdGenerator();
		private readonly CancellationTokenSource source = new CancellationTokenSource();

		public Task<SubscriptionId> Subscribe(TCriteria criteria,
			Action<TNextArgument> onNextAction, Action<Exception> errorAction)
		{
			return Subscribe(criteria, new ReactiveActions<TNextArgument>(onNextAction, errorAction));
		}

		private TResult SafeWait<TResult>(Func<TResult> functionToCall)
		{
			try
			{
				slimSemaphore.Wait(source.Token);
				return functionToCall();
			}
			finally
			{
				slimSemaphore.Release();
			}
		}

		private void SafeWait(Action a)
		{
			SafeWait(() =>
			{
				a();
				return this;
			});
		}


		public async Task<SubscriptionId> Subscribe(TCriteria criteria, ReactiveActions<TNextArgument> actions)
		{
			var idTask = SafeWait(async () =>
			{
				if (subscriptions.ContainsKey(criteria))
				{
					subscriptions[criteria].IncreaseCounter();
					return subscriptions[criteria].SubscriptionId;
				}

				var disposable = await SubscribeInternal(criteria, actions).ConfigureAwait(false);
				var generatedId = generator.GetNewId();
				var virtualSubscription = new VirtualSubscription(generatedId, disposable);
				subscriptions.Add(criteria, virtualSubscription);
				return generatedId;
			});

			var subscriptionId = await idTask.ConfigureAwait(false);
			return subscriptionId;
		}


		public bool Unsubscribe(SubscriptionId id)
		{
			var result = SafeWait(() =>
			{
				foreach (var subscription in subscriptions)
				{
					var virtualSubscription = subscription.Value;
					if (virtualSubscription.Has(id))
					{
						ProcessUnsubscribe(subscription.Key, subscription.Value);
						return true;
					}
				}

				return false;
			});

			return result;
		}


		private void ProcessUnsubscribe(TCriteria criteria, VirtualSubscription virtualSubscription)
		{
			virtualSubscription.DecreaseCounter();
			if (virtualSubscription.NoMoreSubscribers())
				subscriptions.Remove(criteria);
		}

		public void Dispose()
		{
			if (source.IsCancellationRequested)
				return;


			SafeWait(() =>
			{
				source.Cancel();
				foreach (var virtualSubscription in subscriptions)
					virtualSubscription.Value.Dispose();
				subscriptions.Clear();
			});
		}
	}
}