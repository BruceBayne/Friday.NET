using System.Security.Principal;

namespace Friday.Base.Routing
{
    public class BasicUserNamePrincipal : IPrincipal
    {
        public IIdentity Identity { get; private set; }
        public bool IsInRole(string role) { return false; }

        public BasicUserNamePrincipal(string userName)
        {
            Identity = new GenericIdentity(userName);
        }
    }
}