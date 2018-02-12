using System;

namespace Friday.Base.Logging
{
	public interface ILogger
	{
		void LogLine(LogLevel level, string line);

		void Trace(string msg);
		void LogDebug(string msg);
		void LogInformation(string msg);
		void LogError(Exception e);
		void LogError(string error);
		void LogError(string error, Exception e);

		void LogCritical(string error);
		void LogCritical(string errorMessage, Exception e);
		void LogCritical(Exception e);
		void LogWarning(string warning);
	}
}