using System;

namespace Friday.Base.Subscriptions.SeparateTask
{
	public class InQueueBlock<TCriteria, TActionArg>
	{
		public readonly TCriteria Criteria;
		public Action<TActionArg> OnNextAction;
		public Action<Exception> ErrorAction;
		public BlockState State { get; private set; }
		public bool IsAlive => State == BlockState.Alive;
		private bool IsSubscribeRequired => State == BlockState.SubscribeRequired;

		public bool CanReceiveUpdates => IsActualForSubscriber && OnNextAction != null;
		public bool CanReceiveErrors => IsActualForSubscriber && ErrorAction != null;


		private bool IsActualForSubscriber => IsAlive || IsSubscribeRequired;


		public override string ToString()
		{
			return $"{nameof(Criteria)}: {Criteria}, {nameof(State)}: {State}";
		}

		private IDisposable subscription;

		public void Dispose()
		{
			subscription?.Dispose();
			State = BlockState.CleanupRequired;
		}

		protected void MarkAsAliveForUnsubscribe(IDisposable disposable)
		{
			OnNextAction = null;
			ErrorAction = null;

			lock (this)
			{
				subscription = disposable;
				State = BlockState.UnsubscribeRequired;
			}
		}

		public void MarkBlockAsNonUsable()
		{
			OnNextAction = null;
			ErrorAction = null;


			lock (this)
			{
				if (subscription != null && IsAlive)
				{
					State = BlockState.UnsubscribeRequired;
					return;
				}

				if (State == BlockState.SubscribeRequired)
					State = BlockState.CleanupRequired;
			}
		}

		public bool TryMarkAsAlive(IDisposable disposable)
		{
			lock (this)
			{
				if (subscription == null && State == BlockState.SubscribeRequired)
				{
					State = BlockState.Alive;
					subscription = disposable;
					return true;
				}


				if (State != BlockState.SubscribeRequired)
					MarkAsAliveForUnsubscribe(disposable);
				return false;
			}
		}



		public InQueueBlock(TCriteria criteria, Action<TActionArg> onNextAction,
			Action<Exception> errorAction)
		{
			Criteria = criteria;
			OnNextAction = onNextAction;
			ErrorAction = errorAction;
			State = BlockState.SubscribeRequired;
		}
	}
}