using System;
using System.Net;

namespace Friday.Base.ValueTypes.Token
{
    [Serializable]
    public class TokenAuthenticationRequest : ITokenAuthenticationRequest
    {
        public AuthToken Token { get; }
        public IPAddress IpAddress { get; }

        public string UserAgent { get; }

        public TokenAuthenticationRequest(AuthToken token, IPAddress ipAddress, string userAgent)
        {
            Token = token;
            IpAddress = ipAddress;
            UserAgent = userAgent;
        }

        public override string ToString()
        {
            return $"{nameof(Token)}: {Token}, {nameof(IpAddress)}: {IpAddress}, {nameof(UserAgent)}: {UserAgent}";
        }

        public static TokenAuthenticationRequest From(AuthToken token, IPAddress ipAddress, string userAgent)
        {
            return new TokenAuthenticationRequest(token, ipAddress, userAgent);
        }
    }
}