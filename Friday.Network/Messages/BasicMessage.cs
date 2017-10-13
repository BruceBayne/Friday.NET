namespace Friday.Network.Messages
{
    public class BasicMessage<TMessageType> where TMessageType: struct 
    {
        public virtual TMessageType MessageType { get; set; }
    }
}
