using System;
using System.Runtime.Serialization.Formatters.Binary;
using Friday.Base.Serialization;

namespace Friday.Network.PacketMaintainers
{
	public class NetworkPacketMaintainer : INetworkPacketMaintainer
	{
		private readonly ITypeExtractor extractor;
		private readonly ICompleteSerializer serializer;

		public NetworkPacketMaintainer(ITypeExtractor extractor, ICompleteSerializer serializer)
		{
			this.extractor = extractor;
			this.serializer = serializer;
		}


		public Type ExtractType(byte[] rawMessage)
		{
			return extractor.ExtractType(rawMessage);
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
