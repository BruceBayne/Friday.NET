using System;

namespace Friday.Base.Subscriptions._Base.DataProvider
{

	public interface IOnExceptionEvent
	{
		event EventHandler<Exception> OnException;
	}
}