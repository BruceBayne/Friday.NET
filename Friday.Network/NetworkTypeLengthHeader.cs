using System;
using System.Runtime.InteropServices;
using Friday.Base.Extensions;

namespace Friday.Network
{
	[StructLayout(LayoutKind.Sequential)]
	public struct NetworkTypeLengthHeader : IEquatable<NetworkTypeLengthHeader>
	{
		public NetworkTypeLengthHeader(uint marker, uint messageType, int messageSize, uint headerHash, uint flags)
		{
			Marker = marker;
			MessageType = messageType;
			MessageSize = messageSize;
			this.headerHash = headerHash;
			this.flags = flags;
		}

		public static int Size => Marshal.SizeOf(typeof(NetworkTypeLengthHeader));
		public const uint ValidHeaderMarker = 0xDEADBEEF;

		public readonly uint Marker;
		public readonly uint MessageType;
		public readonly int MessageSize;
		public readonly uint headerHash;
		public readonly uint flags;

		public static byte[] GenerateHeaderBytesFrom(byte[] value)
		{

			var result = new NetworkTypeLengthHeader(ValidHeaderMarker, 0, value.Length, 0, 0);
			return result.ToByteArray();

		}

		public override string ToString()
		{
			return $"{nameof(MessageType)}: {MessageType}, {nameof(MessageSize)}: {MessageSize}";
		}

		public bool Equals(NetworkTypeLengthHeader other)
		{
			return Marker == other.Marker && MessageType == other.MessageType && MessageSize == other.MessageSize && headerHash == other.headerHash && flags == other.flags;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			return obj is NetworkTypeLengthHeader && Equals((NetworkTypeLengthHeader)obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				var hashCode = (int)Marker;
				hashCode = (hashCode * 397) ^ (int)MessageType;
				hashCode = (hashCode * 397) ^ MessageSize;
				hashCode = (hashCode * 397) ^ (int)headerHash;
				hashCode = (hashCode * 397) ^ (int)flags;
				return hashCode;
			}
		}
	}
}