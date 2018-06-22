using System.Threading.Tasks;

namespace Friday.Base.Routing.Interfaces
{
	/// <summary>
	/// Used with <see cref="RoutingProvider"/>RoutingProvider,
	/// specifies messages that provider can pass to target
	/// </summary>
	/// <typeparam name="TMessage">MessageType to pass</typeparam>
	public interface IMessageHandlerAsync<in TMessage>
	{
		Task HandleMessage(TMessage request);
	}

	/// <summary>
	/// Used with <see cref="RoutingProvider"/>RoutingProvider,
	/// specifies messages that provider can pass to target
	/// </summary>
	/// <typeparam name="TContext">Any context that will passed to handler</typeparam>
	/// <typeparam name="TMessage">MessageType to pass</typeparam>

	public interface IMessageHandlerAsync<in TContext, in TMessage>
	{
		Task HandleMessage(TContext context, TMessage request);
	}

}