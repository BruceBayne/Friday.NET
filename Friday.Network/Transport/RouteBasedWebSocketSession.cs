using System;
using System.Security.Principal;
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
		protected IPrincipal Principal { get; private set; }

		protected IRoutingContext<TServerMessage> RoutingContext { get; private set; }
		protected bool IsAuthenticated => Principal != null;

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

			Principal = null;
		}

		protected sealed override void ProcessMessage(TClientMessage message)
		{
			if (IsAuthenticated)
			{
				ProcessAuthorizedMessage(message);
				return;
			}

			if (!IsAuthenticated)
			{
				if (message is TSignInMessage signInMessage)
					ProcessSignIn(signInMessage);
			}
		}

		private void ProcessSignIn(TSignInMessage signInMessage)
		{
			RoutingContext = authService.LoadContext(signInMessage);
			Send(GetAuthSuccessMessage(signInMessage));
			RoutingContext.OnMessageAvailable += RouterOnMessageAvailable;

		    RoutingContext.Start();
		}

		protected virtual void RouterOnMessageAvailable(object sender, TServerMessage message)
		{
			Send(message);
		}

		private void ProcessAuthorizedMessage(TClientMessage message)
		{
			if (message.GetType() == GetSignOutMessageType())
			{
				CloseSession();
				return;
			}
			RoutingContext?.RouteObject(this, message);
		}


		public void Dispose()
		{
			Cleanup();
		}
	}
}