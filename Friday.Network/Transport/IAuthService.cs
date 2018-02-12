namespace Friday.Network.Transport
{
	public interface IAuthService<in TSignInMessage, TBasicMessage>
	{
		IRoutingContext<TBasicMessage> LoadContext(TSignInMessage signInMessage);
	}
}