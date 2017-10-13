using System;
using System.Threading.Tasks;

namespace Friday.Base.Routing
{
    public class RoutingProvider
    {
        public event EventHandler<Exception> OnProvideRoutingException;



        public RoutingProvider()
        {

        }

        public Task PassThroughRouteAsync(object argument)
        {
            throw new NotImplementedException();
        }

        public Task PassThroughRoute(object argument)
        {
            throw new NotImplementedException();
        }

        protected virtual void DoOnException(Exception e)
        {
            OnProvideRoutingException?.Invoke(this, e);
        }

        public void RegisterRoute()
        {

        }

    }
}
