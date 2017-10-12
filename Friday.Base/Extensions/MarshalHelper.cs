using System.Runtime.InteropServices;

namespace Friday.Base.Extensions
{
    public static class MarshalHelper
    {

        /// <summary>
        /// Convert bytes input to specified structure
        /// </summary>
        /// <typeparam name="T">Structure type</typeparam>
        /// <param name="bytes">Input bytes</param>
        /// <returns></returns>
        public static T ToStructure<T>(this byte[] bytes) where T : struct
        {
            GCHandle handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            T stuff = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(),
                typeof(T));
            handle.Free();
            return stuff;
        }
        public static byte[] ToByteArray<T>(this T obj) where T : struct
        {
            var len = Marshal.SizeOf(obj);
            var arr = new byte[len];
            var ptr = Marshal.AllocHGlobal(len);
            Marshal.StructureToPtr(obj, ptr, true);
            Marshal.Copy(ptr, arr, 0, len);
            Marshal.FreeHGlobal(ptr);
            return arr;
        }


    }
}