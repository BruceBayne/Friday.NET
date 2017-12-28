using System.Security.Principal;

namespace Friday.Base.Routing.Interfaces
{
	public interface IRoutingContext
	{
		IPrincipal Principal { get; set; }

	}
}