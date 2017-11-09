namespace Friday.Base.Network
{
    public interface IMessageType<out T>
    {
        T MessageType { get; }
    }
}