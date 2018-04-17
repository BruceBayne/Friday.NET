using System.Net;

namespace Friday.Base.ValueTypes.Token
{
    public interface ITokenAuthenticationRequest
    {
        IPAddress IpAddress { get; }
        AuthToken Token { get; }
    }
}