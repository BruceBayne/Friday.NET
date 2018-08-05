using System;
using System.Threading.Tasks;

namespace Friday.Base.Scheduler
{
	/// <summary>
	/// Do something with delay
	/// </summary>
	public sealed class Do
	{
		private readonly TimeSpan timeSpan;
		private readonly Func<Task> asyncAction;
		private readonly Action action;

		public Do(TimeSpan timeSpan, Func<Task> asyncAction)
		{
			this.timeSpan = timeSpan;
			this.asyncAction = asyncAction;
		}

		private Do(TimeSpan timeSpan, Action action)
		{
			this.action = action;
			this.timeSpan = timeSpan;
		}


		public static Do Each(TimeSpan timeSpan, Action action)
		{
			var doEach = new Do(timeSpan, action);
			doEach.Run();
			return doEach;
		}


		public static Do EachAsync(TimeSpan timeSpan, Func<Task> action)
		{
			var doEach = new Do(timeSpan, action);
			doEach.Run();

			return doEach;
		}


		private void Run()
		{
			Task.Run(async () =>
			{
				while (true)
				{
					try
					{
						if (asyncAction != null)
							await asyncAction();
						action?.Invoke();

						await Task.Delay(timeSpan);
					}
					catch (Exception e)
					{
						Console.WriteLine(e);
						throw;
					}
				}
			});
		}
	}
}