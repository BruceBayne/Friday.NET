namespace Friday.Network
{
    public class SocketSettings
    {
        /// <summary>
        ///     The network client keep alive interval.
        /// </summary>
        public uint NetworkClientKeepAliveInterval = 1000;

        /// <summary>
        ///     The network client keep alive timeout.
        /// </summary>
        public uint NetworkClientKeepAliveTimeout = 1000;

        /// <summary>
        ///     The network client receive buffer size.
        /// </summary>
        public int NetworkClientReceiveBufferSize = (1024 * 1024) * 1;

        /// <summary>
        ///     The network client receive timeout.
        /// </summary>
        public int NetworkClientReceiveTimeout = 1000 * 60 * 180;

        /// <summary>
        ///     The network client send buffer size.
        /// </summary>
        public int NetworkClientSendBufferSize = (1024 * 1024) * 1;

        /// <summary>
        ///     The network client send timeout.
        /// </summary>
        public int NetworkClientSendTimeout = 1000 * 60 * 180;

        /// <summary>
        ///     Gets or sets a value indicating whether to use No Delay for socket.
        /// </summary>
        public bool UseNoDelay = true;

        /// <summary>
        ///     Initialize the <see cref="SocketSettings" /> class.
        /// </summary>
        public SocketSettings()
        {

        }

        public static SocketSettings Default => new SocketSettings();


        public static SocketSettings DefaultHigh
        {
            get
            {
                var timeOut = 1000 * 60 * 30;
                return new SocketSettings()
                {
                    NetworkClientReceiveTimeout = timeOut,
                    NetworkClientSendTimeout = timeOut,
                    NetworkClientKeepAliveInterval = 1000,
                    UseNoDelay = true,
                    NetworkClientKeepAliveTimeout = 1000,
                };
            }
        }

    }
}