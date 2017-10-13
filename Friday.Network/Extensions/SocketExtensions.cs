using System;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using Friday.Base.Extensions;

namespace Friday.Network.Extensions
{
    public static class SocketExtensions
    {
        public static bool SetKeepAliveValues(this Socket socket, bool onOff, uint keepaLiveTime,
            uint keepAliveInterval)
        {
            var keepAliveValues = new TcpKeepAlive
            {
                OnOff = Convert.ToUInt32(onOff),
                KeepAliveTime = keepaLiveTime,
                KeepAliveInterval = keepAliveInterval,
            };


            try
            {
                socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, onOff);
                int result = socket.IOControl(IOControlCode.KeepAliveValues, keepAliveValues.ToByteArray(), null);
                return result > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }


        /// <summary>
        ///     The tcp keep alive.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        private struct TcpKeepAlive
        {
            /// <summary>
            ///     The on off.
            /// </summary>
            public uint OnOff;

            /// <summary>
            ///     The keep alive time.
            /// </summary>
            public uint KeepAliveTime;

            /// <summary>
            ///     The keep alive interval.
            /// </summary>
            public uint KeepAliveInterval;
        }


        public static void SetupSocketTimeouts(this Socket socket, SocketSettings settings)
        {
            socket.ReceiveTimeout = settings.NetworkClientReceiveTimeout;
            socket.SendTimeout = settings.NetworkClientSendTimeout;
            socket.ReceiveBufferSize = settings.NetworkClientReceiveBufferSize;
            socket.SendBufferSize = settings.NetworkClientSendBufferSize;
            socket.NoDelay = settings.UseNoDelay;


            SetKeepAliveValues(socket, true,
                keepaLiveTime: settings.NetworkClientKeepAliveTimeout,
                keepAliveInterval: settings.NetworkClientKeepAliveInterval);
        }
    }
}