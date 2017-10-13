using System.IO;
using System.Threading.Tasks;

namespace Friday.Network
{
    public static class StreamExtenstions
    {
        public static Task<byte[]> ReadAsyncComplete(this Stream stream, int size)
        {
            return Task.Run(async () =>
            {
                byte[] b = new byte[size];
                int nPos = 0;

                while (nPos != size)
                {
                    int dwDataSize = size - nPos;
                    int readLen = await stream.ReadAsync(b, nPos, dwDataSize);
                    if (readLen < 1)
                        throw new EndOfStreamException("Read Error");
                    nPos += readLen;
                }
                return b;
            });
        }

    }
}