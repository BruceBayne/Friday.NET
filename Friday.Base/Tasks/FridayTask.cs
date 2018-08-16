using System;
using System.Text;
using System.Threading.Tasks;
using Friday.Base.Extensions.Reflection;

namespace Friday.Base.Tasks
{
	/// <summary>
	/// .NET tasks/schedulers is REALLY far from ideal , lets try fix some cases
	/// </summary>
	public static class FridayTask
	{
		private static readonly TaskScheduler RealDefaultTaskScheduller;
		public static FridayTaskScheduler FridayTaskScheduller { get; }

		public static TaskFactory Factory { get; }


		/// <summary>
		/// Prevents UnobservedTaskException
		/// </summary>
		public static void ContinueWithDefaultExceptionHandler(this Task task, Action<Task> continuationAction)
		{
			AttachDefaultExceptionHandler(task.ContinueWith(continuationAction));
		}




		/// <summary>
		/// Prevents UnobservedTaskException
		/// </summary>
		public static void AttachDefaultExceptionHandler(this Task task)
		{
			task.ContinueWith(task1 => FridayDebugger.LogFailedTask(task), TaskContinuationOptions.OnlyOnFaulted).ConfigureAwait(false);
		}




		static FridayTask()
		{
			RealDefaultTaskScheduller = TaskScheduler.Default;
			FridayTaskScheduller = new FridayTaskScheduler(RealDefaultTaskScheduller);
			Factory = new TaskFactory(FridayTaskScheduller);
		}


		public static bool TryRestoreDefaultTaskScheduler()
		{
			return TryReplaceDefaultTaskScheduler(RealDefaultTaskScheduller);
		}


		public static bool TryReplaceDefaultTaskScheduler(TaskScheduler newScheduler)
		{
			return ReflectionExtensions.TryReplaceBackingField<TaskScheduler>("s_defaultTaskScheduler", newScheduler);
		}

		public static bool SetupFridaySchedulerAsDefault()
		{
			return TryReplaceDefaultTaskScheduler(FridayTaskScheduller);
		}
	}
}