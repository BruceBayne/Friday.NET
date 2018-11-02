using System;
using System.Threading.Tasks;

namespace Friday.Base.Tasks.Combinators
{
	public static class TaskCombinatorExtensions
	{
		public static async Task<T> WithTimeout<T>(this Task<T> task, TimeSpan delay)
		{
			var delayTask = Task.Delay(delay);
			var firstToFinish = await Task.WhenAny(task, delayTask).ConfigureAwait(false);

			if (firstToFinish == delayTask)
			{
				task.AttachDefaultExceptionHandler();
				throw new TimeoutException();
			}

			return task.Result;
		}


		/// <summary>
		/// Retry task execution on failed, each 5 Seconds 
		/// </summary>
		/// <typeparam name="T">Task result</typeparam>
		/// <param name="task">Task to retry</param>
		/// <param name="attempts">Maximum attempts to retry</param>
		/// <returns></returns>
		public static Task<T> WithRetriesOnFail<T>(this Task<T> task, int attempts = 3)
		{
			return WithRetriesOnFail(task, TimeSpan.FromSeconds(5), attempts);
		}


		public static async Task<T> WithRetriesOnFail<T>(this Task<T> task, TimeSpan delayBetweenAttempts, int attempts = 3)
		{
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