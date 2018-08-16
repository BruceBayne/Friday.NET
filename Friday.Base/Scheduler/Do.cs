using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Friday.Base.Scheduler
{

	/// <summary>
	/// Do something with delay
	/// </summary>
	public sealed class Do
	{
		private readonly TimeSpan timeSpan;
		private Func<Task> asyncAction;
		private Action action;
		private readonly Action<Exception> exceptionAction;


		private readonly CancellationToken token;
		private Task innerTask;

		public Do(TimeSpan timeSpan, Func<Task> asyncAction, Action action, CancellationToken token,
			Action<Exception> exceptionAction = null)
		{
			this.timeSpan = timeSpan;
			this.asyncAction = asyncAction;
			this.token = token;
			this.exceptionAction = exceptionAction;
			this.action = action;
		}


		public Do(TimeSpan timeSpan, Func<Task> asyncAction, CancellationToken token,
			Action<Exception> exceptionAction = null) : this(timeSpan, asyncAction, null, token, exceptionAction)
		{

		}



		public Do(TimeSpan timeSpan, Func<Task> asyncAction, Action<Exception> exceptionAction = null) : this(timeSpan, asyncAction, null, CancellationToken.None, exceptionAction)
		{

		}

		public Do(TimeSpan timeSpan, Action action, Action<Exception> exceptionAction = null) : this(timeSpan, null, action, CancellationToken.None, exceptionAction)
		{

		}



		public Do(TimeSpan timeSpan, Action action, CancellationToken token,
			Action<Exception> exceptionAction = null) : this(timeSpan, null, action, token, exceptionAction)
		{

		}


		public static Do Every(TimeSpan timeSpan, Action action, CancellationToken token,
			Action<Exception> exceptionAction = null)
		{
			var doEvery = new Do(timeSpan, null, action, token, exceptionAction);
			doEvery.Run();
			return doEvery;
		}


		public static Do Every(TimeSpan timeSpan, Action action, Action<Exception> exceptionAction = null)
		{
			var doEvery = new Do(timeSpan, null, action, CancellationToken.None, exceptionAction);
			doEvery.Run();
			return doEvery;
		}


		public static Do EveryAsync(TimeSpan timeSpan, Func<Task> action)
		{
			var doEvery = new Do(timeSpan, action);
			doEvery.Run();
			return doEvery;
		}

		public static Do EveryAsync(TimeSpan timeSpan, Func<Task> action, CancellationToken token)
		{
			var doEvery = new Do(timeSpan, action, token);
			doEvery.Run();
			return doEvery;
		}


		public static Do EveryAsync(TimeSpan timeSpan, Func<Task> action, CancellationToken token,
			Action<Exception> onException)
		{
			var doEvery = new Do(timeSpan, action, token, onException);
			doEvery.Run();
			return doEvery;
		}


		public void Stop()
		{
			if (innerTask == null)
				return;

			innerTask = null;
			asyncAction = null;
			action = null;
		}


		private void Run()
		{
			innerTask = Task.Run(async () =>
			{
				while (ShouldContinueWork())
				{
					try
					{
						if (asyncAction != null)
							await asyncAction();
						action?.Invoke();
						await Task.Delay(timeSpan, token);
					}

					catch (OperationCanceledException)
					{
						return;
					}
					catch (Exception e)
					{
						exceptionAction?.Invoke(e);

						if (exceptionAction == null)
							FridayDebugger.Log(e);
					}
				}
			}, token);
		}

		private bool ShouldContinueWork()
		{
			return !token.IsCancellationRequested && (action != null || asyncAction != null);
		}
	}
}