using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Friday.Base.Tasks
{
	/// <summary>
	/// This scheduler has global Task Counter
	/// Also provide methods Before/After TaskExecution
	/// </summary>
	public class FridayTaskScheduler : TaskScheduler
	{
		private static int enqueuedTasks = 0;
		public static int EnqueuedTasks => enqueuedTasks;

		private static int currentlyExecutingTasks = 0;
		public static int CurrentlyExecutingTasks => currentlyExecutingTasks;


		/// <summary>
		/// Its highly recommended to protect this action using custom try/catch handlers
		/// otherwise UnhandledTaskException occur's
		/// </summary>
		public static Action<Task> BeforeTaskExecution { get; set; }

		/// <summary>
		/// Its highly recommended to protect this action using custom try/catch handlers
		/// otherwise UnhandledTaskException occur's
		/// </summary>
		public static Action<Task> AfterTaskExecution { get; set; }

		private readonly TaskScheduler threadPoolTaskScheduler;
		private readonly MethodInfo schedTasks;
		private readonly MethodInfo queMethod;


		public FridayTaskScheduler(TaskScheduler threadPoolTaskScheduler)
		{
			this.threadPoolTaskScheduler = threadPoolTaskScheduler;

			const BindingFlags bindingAttr = BindingFlags.NonPublic | BindingFlags.Instance;
			schedTasks = threadPoolTaskScheduler.GetType().GetMethod(nameof(GetScheduledTasks), bindingAttr);
			queMethod = threadPoolTaskScheduler.GetType().GetMethod(nameof(QueueTask), bindingAttr);
		}


		protected override IEnumerable<Task> GetScheduledTasks()
		{
			var scheduledTasks = (IEnumerable<Task>)schedTasks.Invoke(threadPoolTaskScheduler, null);
			return scheduledTasks;
		}

		protected override bool TryDequeue(Task task)
		{
			return base.TryDequeue(task);
		}


		protected override void QueueTask(Task task)
		{
			var proxyTask = new Task(() =>
			{
				Interlocked.Increment(ref currentlyExecutingTasks);

				BeforeTaskExecution?.Invoke(task);
				if (TryExecuteTask(task))
					Interlocked.Decrement(ref enqueuedTasks);
				AfterTaskExecution?.Invoke(task);
				Interlocked.Decrement(ref currentlyExecutingTasks);
			});

			Interlocked.Increment(ref enqueuedTasks);
			UseOriginalSchedulerQueueTask(proxyTask);
		}

		protected void UseOriginalSchedulerQueueTask(Task proxyTask)
		{
			queMethod.Invoke(threadPoolTaskScheduler, new object[] { proxyTask });
		}

		protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
		{
			return TryExecuteTask(task);
		}
	}
}