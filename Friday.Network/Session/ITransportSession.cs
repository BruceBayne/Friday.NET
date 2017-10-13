using System;
using System.Threading.Tasks;
using Friday.Network.Messages;

namespace Friday.Network.Session
{
    public interface ITransportSession
    {
        event EventHandler<Exception> OnException;
        event EventHandler<ITransportSession> OnConnected;
        event EventHandler<ITransportSession> OnDisconnect;



        Task SendMessageAsync(IOutgoingMessageMarker message);
        Task CloseConnectionAsync();

        Task ProcessMessagesAsync();

        Task SendMessage(IOutgoingMessageMarker message);
        Task CloseConnection();
        void AbortConnection();

    }
}