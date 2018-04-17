using System;
using System.Threading.Tasks;
using Friday.Base.Network;
using Friday.Base.Serialization.Readable;
using WebSocketSharp;

namespace Friday.Network.Transport
{
    public abstract class
        RouteBasedWebSocketSession<TServerMessage, TClientMessage, TClientMessageType, TSignInMessage> :
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


            if (message is TSignInMessage signInMessage)
            {
                await ProcessSignIn(signInMessage);
                return;
            }

            await ProcessUnauthorizedMessage(message);
        }

        protected virtual Task ProcessUnauthorizedMessage(TClientMessage message)
        {
            return Task.CompletedTask;
        }

        protected virtual async Task ProcessSignIn(TSignInMessage signInMessage)
        {
            if (IsAuthenticated)
                return;

            RoutingContext = await authService.LoadContext(signInMessage, out var responseMessage);
            SendMessage(responseMessage);
            RoutingContext.OnMessageAvailable += RouterOnMessageAvailable;
            RoutingContext.Start();
        }

        protected virtual void RouterOnMessageAvailable(object sender, TServerMessage message)
        {
            SendMessage(message);
        }

        private async Task ProcessAuthorizedMessage(TClientMessage message)
        {
            if (!IsAuthenticated)
                return;

            if (message.GetType() == GetSignOutMessageType())
            {
                CloseSession();
                return;
            }

            MessageReadyToBeRouted(message);
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