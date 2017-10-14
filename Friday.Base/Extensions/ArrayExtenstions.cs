using System;
using System.Linq;
using System.Text;

namespace Friday.Base.Extensions
{
    public static class ArrayExtenstions
    {

        public static ArraySegment<byte> ToArraySegment(this byte[] b)
        {
            return new ArraySegment<byte>(b);

        }

        public static string ToHexString(this byte[] inputBytes)
        {
            if (inputBytes == null)
                return string.Empty;

            return string.Join(string.Empty, inputBytes.Select(t => t.ToString("x2")));
        }

        public static string ToStringData(this ArraySegment<byte> inputBytes)
        {
            return Encoding.UTF8.GetString(inputBytes.Array, 0, inputBytes.Count);
        }

        public static string ToStringData(this byte[] inputBytes)
        {
            return Encoding.UTF8.GetString(inputBytes);
        }
    }
}
