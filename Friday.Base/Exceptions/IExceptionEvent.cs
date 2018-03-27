using System;

namespace Friday.Base.Exceptions
{
	public interface IExceptionEvent
	{
		event EventHandler<Exception> OnException;
	}
}