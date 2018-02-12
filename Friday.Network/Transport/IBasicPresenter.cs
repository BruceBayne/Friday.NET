using System;

namespace Friday.Network.Transport
{
	public interface IBasicPresenter<TPresenterSnapshot>
	{
		event EventHandler<TPresenterSnapshot> OnPresenterChanged;
		TPresenterSnapshot GetPresentation();
	}
}