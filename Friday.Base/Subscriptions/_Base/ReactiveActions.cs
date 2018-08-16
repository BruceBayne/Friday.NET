using System;

namespace Friday.Base.Subscriptions._Base
{
	public class ReactiveActions<TActionArg>
	{
		public Action<TActionArg> NextAction { get; }
		public Action<Exception> ErrorAction { get; }

		public ReactiveActions(Action<TActionArg> nextAction, Action<Exception> errorAction = null)
		{
			NextAction = nextAction;
			ErrorAction = errorAction;
		}
	}
}