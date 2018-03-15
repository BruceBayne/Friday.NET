using System;

namespace Friday.Base.Logging
{

	public interface ILogger
	{
		void LogLine(LogLevel level, string line);


		/// <summary>
		/// Tracing information and debugging minutiae; generally only switched on in unusual situations
		/// </summary>
		void Trace(string msg);

		/// <summary>
		/// Debug - internal control flow and diagnostic state dumps to facilitate pinpointing of recognized problems
		/// </summary>
		/// <param name="msg"></param>
		void LogDebug(string msg);

		/// <summary>
		///Information - events of interest or that have relevance to outside observers; the default enabled minimum logging level
		/// </summary>
		/// <param name="msg"></param>
		void LogInformation(string msg);


		/// <summary>
		/// Error - indicating a failure within the application or connected system
		/// </summary>
		/// <param name="e"></param>
		void LogError(Exception e);

		/// <summary>
		/// Error - indicating a failure within the application or connected system
		/// </summary>
		/// <param name="error"></param>
		void LogError(string error);

		/// <summary>
		/// Error - indicating a failure within the application or connected system
		/// </summary>
		/// <param name="error"></param>
		/// <param name="e"></param>
		void LogError(string error, Exception e);


		/// <summary>
		/// Critical errors causing complete failure of the application
		/// </summary>
		/// <param name="error"></param>
		void LogCritical(string error);

		/// <summary>
		/// Critical errors causing complete failure of the application
		/// </summary>
		void LogCritical(string errorMessage, Exception e);

		/// <summary>
		/// Critical errors causing complete failure of the application
		/// </summary>
		void LogCritical(Exception e);

		/// <summary>
		/// Warning - indicators of possible issues or service/functionality degradation
		/// </summary>
		/// <param name="warning"></param>
		void LogWarning(string warning);
	}
}