using Friday.Base.Serialization;

namespace Friday.Network.PacketMaintainers
{
    public interface INetworkPacketMaintainer : IMessageTypeDeterminer, ICompleteSerializer
    {
        

    }
}