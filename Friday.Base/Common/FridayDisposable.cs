using System;
using Friday.Base.Extensions.Action;

namespace Friday.Base.Common
{

	public static class DisposableExtensions
	{

	}


	public sealed class FridayDisposable : IDisposable
	{
		private readonly Action action;
		private bool disposed;

		public FridayDisposable(Action action)
		{
			this.action = action;
		}

		public void Dispose()
		{
			if (disposed)
				return;

			disposed = true;
			action();
		}

		public static FridayDisposable Create(Action action)
		{
			return new FridayDisposable(action);
		}
	}
}