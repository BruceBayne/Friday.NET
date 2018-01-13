using System;
using System.Threading.Tasks;

namespace Friday.Base.Routing.Interfaces
{
	public interface IAsyncObjectRouter
	{
		Task RouteCallAsync<T>(Func<T, Task> callAction) where T : class;
		Task RouteObjectAsync(object context, object objectForRoute);
	}
}