using System;

namespace Friday.Base.Interfaces
{
	public interface IOnExceptionEvent
	{
		event EventHandler<Exception> OnException;
	}
}
