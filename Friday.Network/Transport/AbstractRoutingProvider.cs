using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Friday.Base.Routing;

namespace Friday.Network.Transport
{
	public abstract class AbstractRoutingProvider : RoutingProvider
	{
		protected void RegisterRoutes(IEnumerable<object> processors)
		{
			foreach (var presenter in processors)
			{
				var rule = RouteRule.UseInterfaceMessageHandler(presenter);
				RegisterRoute(rule);
			}
		}
	}
}
