using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Friday.Base
{
	public static class FridayDebugger
	{
		public const string LibraryName = "Friday.NET";

		public static void ConditionalBreak(Func<bool> breakCondition)
		{
			if (breakCondition())
				SafeBreak();
		}

		public static void SafeBreak()
		{
			if (Debugger.IsAttached)
				Debugger.Break();
		}


		public static void Log(Task t, Exception exception)
		{


		}


		public static void Log(Exception exception)
		{
			//if (Debugger.IsAttached && exception != null)
			if (exception != null)
				Debugger.Log(0, LibraryName, exception.Message);
		}

		public static void LogFailedTask(Task task)
		{
			if (task.IsFaulted)
				Log(task.Exception);
		}
	}
}