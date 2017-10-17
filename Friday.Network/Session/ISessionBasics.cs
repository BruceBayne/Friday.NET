using Friday.Network.Messages;

namespace Friday.Network.Session
{
    public interface ISessionBasics
    {
        void SendMessage(IOutgoingMessageMarker msg);
        void CloseSession();
    }
}