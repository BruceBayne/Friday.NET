using Friday.Base.Routing;
using Friday.Base.Routing.Attributes;

namespace Friday.Base.Tests.Routing.Environment
{
	internal static class RoutingTestEnvironment
	{
		internal static SomeDto GetTestDtoData()
		{
			return new SomeDto();
		}


		internal static (RoutingProvider provider, AuthRequiredApiProcessor processor) GetAuthProviderWithProcessor()
		{
			var processor = new AuthRequiredApiProcessor();
			var provider = GetStaticNamesRoutingProvider(processor);
			provider.RegisterAttributeHandler(new AuthRequiredAttributeHandler());
			return (provider, processor);
		}


		internal static RoutingProvider GetAuthProvider()
		{
			return GetAuthProviderWithProcessor().provider;
		}


		internal static RoutingProvider GetInterfaceBasedRoutingProvider(params object[] processors)
		{
			var router = new RoutingProvider();
			foreach (var processor in processors)
				router.RegisterRoute(new RouteRule(processor, RouteOptions.UseInterfaceMessageHandler()));
			return router;
		}

		internal static RoutingProvider GetStaticNamesRoutingProvider(params object[] processors)
		{
			var provider = new RoutingProvider();
			foreach (var processor in processors)
				provider.RegisterRoute(new RouteRule(processor, RouteOptions.UseStatic("On{typeName}")));

			return provider;
		}
	}
}