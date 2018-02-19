using System;

namespace Friday.Network.Transport
{
	public interface IBasicPresenter<TPresenterSnapshot>
	{

	    void Start();
	    void Dispose();

		event EventHandler<TPresenterSnapshot> OnPresenterChanged;
		TPresenterSnapshot GetPresentation();
	}
}