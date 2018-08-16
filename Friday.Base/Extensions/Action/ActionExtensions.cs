using System;
using System.Collections.Generic;
using System.Text;
using Friday.Base.Common;
using Friday.Base.Subscriptions.ToDisposable;

namespace Friday.Base.Extensions.Action
{
	public static class ActionExtensions
	{

		public static IDisposable ToDisposable(this System.Action a)
		{
			return FridayDisposable.Create(a);
		}

	}
}
