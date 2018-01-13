namespace Friday.Base.Routing.Interfaces
{
	/// <summary>
	/// Used with <see cref="RoutingProvider"/>RoutingProvider,
	/// specifies messages that provider can pass to target
	/// </summary>
	/// <typeparam name="TMessage">MessageType to pass</typeparam>
	public interface IMessageHandler<in TMessage>
	{
		void HandleMessage(TMessage message);
	}


	/// <summary>
	/// Used with <see cref="RoutingProvider"/>RoutingProvider,
	/// specifies messages that provider can pass to target
	/// </summary>
	/// <typeparam name="TContext">Router/caller/e.t.c </typeparam>
	/// <typeparam name="TMessage">MessageType to pass</typeparam>
	public interface IMessageHandler<in TContext, in TMessage>
	{
		void HandleMessage(TContext sender, TMessage message);
	}

}