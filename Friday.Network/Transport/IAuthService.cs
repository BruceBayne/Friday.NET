using System.Threading.Tasks;

namespace Friday.Network.Transport
{
    public interface IAuthService<in TSignInMessage, TBasicMessage>
    {
        Task<IRoutingContext<TBasicMessage>> LoadContext(TSignInMessage signInMessage, out TBasicMessage serverMessage);
    }
}