using System.Runtime.InteropServices;
using Friday.Base.Extensions;

namespace Friday.Network
{
    [StructLayout(LayoutKind.Sequential)]
    public struct NetworkTypeLengthHeader
    {
        public static int Size => Marshal.SizeOf(typeof(NetworkTypeLengthHeader));
        public const uint ValidHeaderMarker = 0xDEADBEEF;

        public uint Marker;
        public uint MessageType;
        public int MessageSize;
        public uint headerHash;
        public uint flags;

        public static byte[] GenerateHeaderBytesFrom(byte[] value)
        {

            NetworkTypeLengthHeader result;
            result.MessageSize = value.Length;
            result.flags = 0;
            result.headerHash = 0;
            result.MessageType = 0;
            result.Marker = ValidHeaderMarker;
            return result.ToByteArray();

        }

        public override string ToString()
        {
            return $"{nameof(MessageType)}: {MessageType}, {nameof(MessageSize)}: {MessageSize}";
        }
    }
}