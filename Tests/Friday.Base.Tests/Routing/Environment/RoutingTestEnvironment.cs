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


		internal static RoutingProvider GetStaticRoutingWithInterfaceRoutingProvider(params object[] processors)
		{
			var router = new RoutingProvider();
			foreach (var processor in processors)
			{
				router.RegisterRoute(new RouteRule(processor, RouteOptions.UseStaticDefaultTemplate()));
				router.RegisterRoute(RouteRule.UseInterfaceMessageHandler(processor));

			}

			return router;
		}



		internal static RoutingProvider GetInterfaceBasedRoutingProvider(params object[] processors)
		{
			var router = new RoutingProvider();
			foreach (var processor in processors)
				router.RegisterRoute(RouteRule.UseInterfaceMessageHandler(processor));
			return router;
		}

		internal static RoutingProvider GetStaticNamesRoutingProvider(params object[] processors)
		{
			var provider = new RoutingProvider();
			foreach (var processor in processors)
				provider.RegisterRoute(new RouteRule(processor, RouteOptions.UseStaticDefaultTemplate()));

			return provider;
		}
	}
}