using System.Diagnostics;

namespace Friday.Service.Basics.Statistics
{
	public static class CurrentProcess
	{
		private static readonly Process Process;

		public static int ThreadCount => Process.Threads.Count;
		public static double WorkingSetMBytes => Process.WorkingSet64 / 1024.0 / 1024.0;


		static CurrentProcess()
		{
			Process = Process.GetCurrentProcess();
		}
	}
}