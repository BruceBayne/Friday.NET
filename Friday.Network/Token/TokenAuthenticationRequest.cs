using System;
using System.Net;

namespace Friday.Network.Token
{
    [Serializable]
    public class TokenAuthenticationRequest : ITokenAuthenticationRequest
    {
        public AuthToken Token { get; }
        public IPAddress IpAddress { get; }

        public TokenAuthenticationRequest(AuthToken token, IPAddress ipAddress)
        {
            Token = token;
            IpAddress = ipAddress;
        }


        public static TokenAuthenticationRequest From(AuthToken token, IPAddress address)
        {
            return new TokenAuthenticationRequest(token, address);
        }

        public static TokenAuthenticationRequest From(AuthToken token)
        {
            return new TokenAuthenticationRequest(token, IPAddress.Any);
        }
    }
}