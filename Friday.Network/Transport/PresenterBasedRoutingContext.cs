using System;
using System.Collections.Generic;
using System.Linq;
using Friday.Base;

namespace Friday.Network.Transport
{
    public abstract class PresenterBasedRoutingContext<TPresenter, TServerMessageBase> : AbstractRoutingProvider,
        IStartableMarker, IDisposable
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
            foreach (var presenter in presenters)
                presenter.OnPresenterChanged -= WebPresenterMarkerOnOnPresenterChanged;
        }

        public void Start()
        {
            foreach (var webPresenterMarker in presenters)
            {
                var presentation = webPresenterMarker.GetPresentation();
                DoMessageAvailable(presentation);
                webPresenterMarker.OnPresenterChanged += WebPresenterMarkerOnOnPresenterChanged;
            }
        }
    }
}