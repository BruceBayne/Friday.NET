using System;

namespace Friday.Network.PacketMaintainers
{
    public interface IMessageTypeDeterminer
    {
        Type GetMessageType(byte[] rawMessage);
    }

    public interface IMessageTypeDeterminer<T>
    {
        Type GetMessageType(byte[] rawMessage);
    }
}