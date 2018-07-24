using System;

namespace Friday.Network.PacketMaintainers
{
	public interface ITypeExtractor
	{
		Type ExtractType(byte[] rawMessage);
	}
}