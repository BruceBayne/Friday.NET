using System;

namespace Friday.Base
{
	public static class Uptime
	{
		public static readonly DateTime StartedAt;

		static Uptime()
		{
			StartedAt = DateTime.UtcNow;
		}

		public static string FullUptime
		{
			get
			{
				var diff = DateTime.UtcNow - StartedAt;
				return diff.ToString(@"d\.hh\:mm\:ss");
			}
		}
	}
}