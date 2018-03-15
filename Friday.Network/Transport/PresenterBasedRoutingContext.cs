using System;
using System.Collections.Generic;
using System.Linq;
using Friday.Base;

namespace Friday.Network.Transport
{
	public abstract class PresenterBasedRoutingContext<TPresenter, TServerMessageBase> : AbstractRoutingProvider,
		IDisposable, IRoutingContext<TServerMessageBase>
		where TPresenter : class, IBasicPresenter<TServerMessageBase>
	{
		private readonly IReadOnlyCollection<TPresenter> presenters;
		public event EventHandler<TServerMessageBase> OnMessageAvailable;


		protected PresenterBasedRoutingContext(IReadOnlyCollection<TPresenter> presenters)
		{
			this.presenters = presenters.ToList();
			RegisterRoutes(presenters);
		}


		private void WebPresenterMarkerOnOnPresenterChanged(object sender, TServerMessageBase e)
		{
			DoMessageAvailable(e);
		}

		private void DoMessageAvailable(TServerMessageBase e)
		{
			if (e != null)
				OnMessageAvailable?.Invoke(this, e);
		}

		public void Dispose()
		{
			DisposePresenters();
		}

		private void DisposePresenters()
		{
			foreach (var presenter in presenters)
			{
				presenter.OnPresenterChanged -= WebPresenterMarkerOnOnPresenterChanged;
				presenter.Dispose();
			}
		}

		public virtual void Start()
		{
			foreach (var webPresenterMarker in presenters)
			{
				webPresenterMarker.Start();

				var presentation = webPresenterMarker.GetPresentation();
				DoMessageAvailable(presentation);
				webPresenterMarker.OnPresenterChanged += WebPresenterMarkerOnOnPresenterChanged;
			}
		}
	}
}