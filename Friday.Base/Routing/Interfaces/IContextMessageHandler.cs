namespace Friday.Base.Routing.Interfaces
{
	public interface IContextMessageHandler<in TMessage>
	{
		void HandleMessage(IRoutingContext context, TMessage message);
	}
}