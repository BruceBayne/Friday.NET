using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Friday.Base.Common;

namespace Friday.Base.Tasks
{
	/// <summary>
	/// Prevent same task to be executed many times 
	/// returning same tasks on same key
	/// </summary>
	/// <typeparam name="TKey">Key for duplicate check</typeparam>
	/// <typeparam name="TValue">Task argument</typeparam>
	public class TaskDuplicateRemover<TKey, TValue> where TKey : struct
	{
		private readonly ConcurrentDictionary<TKey, Task<TValue>> elements =
			new ConcurrentDictionary<TKey, Task<TValue>>();

		private readonly HashCodeSynchronization synchronization = new HashCodeSynchronization();

		/// <summary>
		/// Create new task or return exists based on key
		/// </summary>
		/// <param name="key">Lookup key</param>
		/// <param name="createFunc">Factory function</param>
		/// <returns>Newly created task or existing task</returns>
		public Task<TValue> GetOrCreateTask(TKey key, Func<Task<TValue>> createFunc)
		{
			lock (synchronization.GetSyncRoot(key)) //Did you know that GetOrAdd has Side effects?
			{
				var task =
					elements.GetOrAdd(key, criteria =>
					{
						var newTask = createFunc();

						newTask.ContinueWithDefaultExceptionHandler(completedTask => { elements.TryRemove(key, out _); });
						return newTask;
					});

				return task;
			}
		}
	}
}