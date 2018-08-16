using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Friday.Base.Common;
using Friday.Base.Subscriptions.ToDisposable;
using Friday.Base.Subscriptions._Base.DataProvider;

namespace Friday.Base.Subscriptions.SeparateTask
{
	/// <summary>
	/// Process all subscription's/unSubscriptions in separate LongRunning task
	/// in parallel way (1 task per subscription)
	/// This can be useful when underlying subscriber is very slow 
	/// </summary>
	public class GenericSeparateTaskSubscriber<TCriteria, TActionArg> : ISubscriber<TCriteria, TActionArg> where TCriteria : IEquatable<TCriteria>
	{
		private readonly ISubscriber<TCriteria, TActionArg> provider;
		private readonly TimeSpan timeBetweenIterations;

		private readonly List<InQueueBlock<TCriteria, TActionArg>> actualBlocks =
			new List<InQueueBlock<TCriteria, TActionArg>>();

		private readonly CancellationTokenSource source = new CancellationTokenSource();
		private Task innerTask;



		public GenericSeparateTaskSubscriber(ISubscriber<TCriteria, TActionArg> provider)
		{
			this.provider = provider;
			this.timeBetweenIterations = TimeSpan.FromSeconds(15);
		}


		public GenericSeparateTaskSubscriber(ISubscriber<TCriteria, TActionArg> provider,
			TimeSpan timeBetweenIterations)
		{
			this.provider = provider;
			this.timeBetweenIterations = timeBetweenIterations;
		}

		public GenericSeparateTaskSubscriber<TCriteria, TActionArg> Start()
		{
			if (innerTask != null)
				throw new Exception("Start called twice");

			innerTask = Task.Factory.StartNew(ProcessSubscriptionsTask, TaskCreationOptions.LongRunning);
			return this;
		}

		public void Dispose()
		{
			source.Cancel();
		}

		private async void ProcessSubscriptionsTask()
		{
			while (!source.IsCancellationRequested)
			{
				try
				{
					var blocksToProcess = GetSafeCopy();
					await ProcessSubscriptionsTasks(blocksToProcess).ConfigureAwait(false);
					ProcessUnSubscriptionTasks(blocksToProcess);
					ProcessCleanupTasks();
					await Task.Delay(timeBetweenIterations, source.Token).ConfigureAwait(false);
				}
				catch (Exception e)
				{
					NotifyException(e);
				}
			}
		}

		private async Task ProcessSubscriptionsTasks(IEnumerable<InQueueBlock<TCriteria, TActionArg>> blocksToProcess)
		{
			var subscribeRequiredBlocks = blocksToProcess.Where(x => x.State == BlockState.SubscribeRequired);
			var tasks = subscribeRequiredBlocks.Select(SubscribeBlock);
			try
			{
				await Task.WhenAll(tasks).ConfigureAwait(false);
			}
			catch (Exception e)
			{
				NotifyException(e);
			}
		}

		private async Task SubscribeBlock(InQueueBlock<TCriteria, TActionArg> block)
		{
			try
			{
				var disposable = await provider.Subscribe(block.Criteria, candle =>
					{
						if (block.CanReceiveUpdates)
							block.OnNextAction(candle);
					}, error => { ReportBlockError(block, error); })
					.ConfigureAwait(false);

				block.TryMarkAsAlive(disposable);
			}
			catch (Exception e)
			{
				if (block.CanReceiveErrors)
				{
					ReportBlockError(block, e);
				}
				else
					NotifyException(e);
			}
		}

		private static void ReportBlockError(InQueueBlock<TCriteria, TActionArg> block, Exception error)
		{
			if (block.CanReceiveErrors)
				block.ErrorAction(error);
		}

		private void ProcessUnSubscriptionTasks(IEnumerable<InQueueBlock<TCriteria, TActionArg>> blocksToProcess)
		{
			try
			{
				var blocks = blocksToProcess.Where(x => x.State == BlockState.UnsubscribeRequired)
					.OrderBy(x => new Guid());

				foreach (var block in blocks)
					block.Dispose();
			}
			catch (Exception e)
			{
				NotifyException(e);
			}
		}


		public event EventHandler<Exception> OnException;

		private void NotifyException(Exception exception)
		{
			OnException?.Invoke(this, exception);
		}

		private void ProcessCleanupTasks()
		{
			lock (actualBlocks)
			{
				actualBlocks.RemoveAll(block => block.State == BlockState.CleanupRequired);
			}
		}

		private List<InQueueBlock<TCriteria, TActionArg>> GetSafeCopy()
		{
			List<InQueueBlock<TCriteria, TActionArg>> blocksCopy;
			lock (actualBlocks)
				blocksCopy = actualBlocks.ToList();
			return blocksCopy;
		}

		public Task<IDisposable> Subscribe(TCriteria criteria,
			Action<TActionArg> onNextAction, Action<Exception> errorAction)
		{
			var block = new InQueueBlock<TCriteria, TActionArg>(criteria, onNextAction, errorAction);

			var disposable = new FridayDisposable(() => { block.MarkBlockAsNonUsable(); });

			lock (actualBlocks)
				actualBlocks.Add(block);

			return Task.FromResult<IDisposable>(disposable);
		}
	}
}