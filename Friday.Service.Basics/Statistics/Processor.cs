using System;
using System.Diagnostics;
using Friday.Base.ValueTypes;

namespace Friday.Service.Basics.Statistics
{
	public static class Processor
	{
		private static readonly TimeSpan SampleFrequency = TimeSpan.FromSeconds(1);
		private static readonly object SyncLock = new object();
		private static readonly PerformanceCounter Counter;
		private static float lastSample;
		private static DateTime lastSampleTime;

		/// <summary>
		/// 
		/// </summary>
		static Processor()
		{
			Counter = new PerformanceCounter("Processor", "% Processor Time", "_Total", true);
		}

		/// <summary>
		/// Get total CPU usage in percents
		/// </summary>
		/// <returns></returns>
		public static Percent TotalPercentLoad()
		{
			if (DateTime.UtcNow - lastSampleTime > SampleFrequency)
			{
				lock (SyncLock)
				{
					if ((DateTime.UtcNow - lastSampleTime) > SampleFrequency)
					{
						lastSample = Counter.NextValue();
						lastSampleTime = DateTime.UtcNow;
					}
				}
			}
			return Percent.From(Math.Round((decimal)lastSample, 2));
		}
	}
}