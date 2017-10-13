using System;
using Friday.Base.Serialization;

namespace Friday.Network.PacketMaintainers
{
    public class NetworkPacketMaintainer : INetworkPacketMaintainer
    {
        private readonly IMessageTypeDeterminer determiner;
        private readonly ICompleteSerializer serializer;

        public NetworkPacketMaintainer(IMessageTypeDeterminer determiner, ICompleteSerializer serializer)
        {
            this.determiner = determiner;
            this.serializer = serializer;
        }


        public Type GetMessageType(byte[] rawMessage)
        {
            return determiner.GetMessageType(rawMessage);
        }

        public byte[] Serialize(object packet)
        {

            return serializer.Serialize(packet);
        }

        public object Deserialize(byte[] buffer, Type type)
        {
            return serializer.Deserialize(buffer, type);
        }

        public T Deserialize<T>(byte[] buffer)
        {
            return serializer.Deserialize<T>(buffer);
        }
    }
}
