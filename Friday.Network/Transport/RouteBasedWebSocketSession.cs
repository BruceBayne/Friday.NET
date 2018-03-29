using System;
using System.Security.Principal;
using System.Threading.Tasks;
using Friday.Base.Network;
using Friday.Base.Serialization.Readable;
using WebSocketSharp;

namespace Friday.Network.Transport
{
	public abstract class RouteBasedWebSocketSession<TServerMessage, TClientMessage, TClientMessageType, TSignInMessage> :
		MessageBasedWebSocketSession<TServerMessage, TClientMessage, TClientMessageType>, IDisposable
		where TClientMessage : IMessageType<TClientMessageType>, new()
		where TSignInMessage : class, TClientMessage
		where TServerMessage : class

	{
		private readonly IAuthService<TSignInMessage, TServerMessage> authService;


		protected IRoutingContext<TServerMessage> RoutingContext { get; private set; }
		protected bool IsAuthenticated => RoutingContext != null;

		protected RouteBasedWebSocketSession(ICompleteReadableSerializer serializer,
			IAuthService<TSignInMessage, TServerMessage> authService) :
			base(serializer)
		{
			this.authService = authService;
		}


		protected abstract Type GetSignOutMessageType();

		protected abstract TServerMessage GetAuthSuccessMessage(TSignInMessage signInMessage);


		protected override void OnClose(CloseEventArgs e)
		{
			base.OnClose(e);

			Cleanup();
		}

		private void Cleanup()
		{
			if (RoutingContext != null)
			{
				RoutingContext.RouteCall<ISessionClosed>(x => x.SessionClosed());
				RoutingContext.OnMessageAvailable -= RouterOnMessageAvailable;
				RoutingContext.Dispose();
			}
			RoutingContext = null;
		}

		protected sealed override async Task ProcessMessage(TClientMessage message)
		{
			if (IsAuthenticated)
			{
				await ProcessAuthorizedMessage(message);
				return;
			}

			if (!IsAuthenticated)
			{
				if (message is TSignInMessage signInMessage)
					ProcessSignIn(signInMessage);
			}
		}

		private async void ProcessSignIn(TSignInMessage signInMessage)
		{
			RoutingContext = await authService.LoadContext(signInMessage);
			Send(GetAuthSuccessMessage(signInMessage));
			RoutingContext.OnMessageAvailable += RouterOnMessageAvailable;

			RoutingContext.Start();
		}

		protected virtual void RouterOnMessageAvailable(object sender, TServerMessage message)
		{
			Send(message);
		}

		private async Task ProcessAuthorizedMessage(TClientMessage message)
		{
			if (message.GetType() == GetSignOutMessageType())
			{
				CloseSession();
				return;
			}


			MessageReadyToBeRouted(message);
			if (RoutingContext != null)
				await RoutingContext.RouteObjectAsync(this, message);
			MessageRoutedSuccessfully(message);
		}

		protected virtual void MessageReadyToBeRouted(TClientMessage message)
		{

		}

		protected virtual void MessageRoutedSuccessfully(TClientMessage message)
		{

		}


		public void Dispose()
		{
			Cleanup();
		}
	}
}