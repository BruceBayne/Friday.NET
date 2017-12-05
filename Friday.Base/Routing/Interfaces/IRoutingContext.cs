using System.Security.Principal;

namespace Friday.Base.Routing
{
    public interface IRoutingContext
    {
         IPrincipal Principal { get; set; }

    }
}