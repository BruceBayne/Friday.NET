using System.Net;

namespace Friday.Network.Token
{
    public interface ITokenAuthenticationRequest
    {
        IPAddress IpAddress { get; }
        AuthToken Token { get; }
    }
}