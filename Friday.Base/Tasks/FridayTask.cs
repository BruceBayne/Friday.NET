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