using System.Reflection;
using Friday.Base.Routing.Exceptions;

namespace Friday.Base.Routing.Attributes
{
    public class AuthRequiredAttributeHandler : RouteAttributeHandler
    {
        public class NotAuthorizedException : RouteValidationException
        {
        }

        public override void Validate(ObjectToRoute obj)
        {
            CheckAttribute(obj, obj.RouteRecord.Processor.GetType().GetCustomAttribute<AuthRequiredAttribute>());
            CheckAttribute(obj, obj.RouteRecord.SelectedMethod.GetCustomAttribute<AuthRequiredAttribute>());
        }

        private static void CheckAttribute(ObjectToRoute obj, AuthRequiredAttribute attrInfo)
        {
            if (attrInfo != null && !IsContextAuthorized(obj.Context))
            {
                throw new NotAuthorizedException();
            }
        }

        private static bool IsContextAuthorized(IRoutingContext context)
        {
            if (context?.Principal?.Identity == null)
                return false;

            return context.Principal.Identity.IsAuthenticated;
        }
    }
}