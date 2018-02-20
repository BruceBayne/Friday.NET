using System.Threading.Tasks;

namespace Friday.Base.Routing.Interfaces
{
	/// <summary>
	/// Used with <see cref="RoutingProvider"/>RoutingProvider,
	/// specifies messages that provider can pass to target
	/// </summary>
	/// <typeparam name="T">MessageType to pass</typeparam>
	public interface IMessageHandlerAsync<in T>
	{
		Task HandleMessage(T request);
	}
}