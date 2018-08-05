using System;
using System.Threading.Tasks;
using Friday.Base.Logging;

namespace Friday.Base.Tasks
{
	public static class TaskCombinatorExtensions
	{
		private static void HandleException(Task task, ILogger logger)
		{
			if (task.Exception != null)
				logger.LogInformation(task.Exception.Message);
		}

		public static async Task<T> WithTimeout<T>(this Task<T> task, TimeSpan delay, ILogger logger)
		{
			var delayTask = Task.Delay(delay);
			var firstToFinish = await Task.WhenAny(task, delayTask).ConfigureAwait(false);

			if (firstToFinish == delayTask)
			{
				await task.ContinueWith(task1 => { HandleException(task1, logger); });
				throw new TimeoutException();
			}

			return await task.ConfigureAwait(false);
		}

		public static async Task<T> WithRetry<T>(this Task<T> task, TimeSpan delayBetweenAttempts, int attempts)
		{
			if (attempts < 1)
				throw new ArgumentException(nameof(attempts));

			do
			{
				try
				{
					var res = await task;
					return res;
				}
				catch (Exception)
				{
					attempts--;
					if (attempts <= 0)
						throw;

					if (delayBetweenAttempts != TimeSpan.Zero)
						await Task.Delay(delayBetweenAttempts).ConfigureAwait(false);
				}
			} while (true);
		}
	}
}