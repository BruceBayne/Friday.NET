using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Friday.Base.Tasks
{
	/// <summary>
	/// Cache task result for specified time period
	/// Thread safe
	/// </summary>
	/// <typeparam name="T">Task result</typeparam>
	public class CachedTask<T>
	{
		private readonly Func<Task<T>> taskFactory;
		private readonly TimeSpan expirationTime;
		public DateTime LastUpdateTime { get; private set; } = DateTime.MinValue;
		private readonly CacheFallbackPolicy policy;
		private TaskCompletionSource<T> innerTask = new TaskCompletionSource<T>();
		public bool ReloadStarted { get; private set; }
		private bool forceReloadRequired;



		public void SetReloadRequired()
		{
			forceReloadRequired = true;
		}


		private void TrySetResult(T result)
		{
			if (innerTask.Task.IsCompleted)
				innerTask = new TaskCompletionSource<T>();


			ReloadStarted = false;
			LastUpdateTime = DateTime.Now;
			innerTask.SetResult(result);
		}

		private void TrySetException(Exception e)
		{
			ReloadStarted = false;


			if (innerTask.Task.IsCompleted)
			{
				var previousTaskSuccess = innerTask.Task.Status == TaskStatus.RanToCompletion;

				if (previousTaskSuccess && policy == CacheFallbackPolicy.LeavePreviousSuccessValue)
					return;

				innerTask = new TaskCompletionSource<T>();
			}

			innerTask.TrySetException(e);
		}


		private async Task ReloadInnerTask()
		{
			try
			{
				var response = await taskFactory().ConfigureAwait(false);
				TrySetResult(response);
			}
			catch (Exception e)
			{
				TrySetException(e);
			}
		}

		public CachedTask(Func<Task<T>> taskFactory, TimeSpan expirationTime,
			CacheFallbackPolicy policy = CacheFallbackPolicy.SetExceptionOnFail)
		{
			if (policy == CacheFallbackPolicy.Invalid)
				throw new ArgumentException(nameof(policy));

			this.taskFactory = taskFactory;
			this.expirationTime = expirationTime;
			this.policy = policy;
			forceReloadRequired = true;
		}

		/// <summary>
		/// Get result from cache
		/// </summary>
		/// <returns></returns>
		public async Task<T> GetResult()
		{
			if (ProbablyReloadRequired())
			{
				lock (this)
				{
					if (ReloadStarted)
						return innerTask.Task.Result;
					ReloadStarted = true;
					forceReloadRequired = false;
				}
				await ReloadInnerTask().ConfigureAwait(false);
			}

			return innerTask.Task.Result;
		}

		private bool ProbablyReloadRequired()
		{
			if (ReloadStarted)
				return false;

			if (forceReloadRequired)
				return true;

			var isCachedTaskExpired = DateTime.Now - LastUpdateTime > expirationTime;
			return isCachedTaskExpired;
		}
	}
}
