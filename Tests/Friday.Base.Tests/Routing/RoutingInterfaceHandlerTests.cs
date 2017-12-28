using System.Threading.Tasks;
using Friday.Base.Routing;
using Friday.Base.Routing.Interfaces;
using Friday.Base.Tests.Routing.Environment;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace Friday.Base.Tests.Routing
{
	[TestClass]
	[TestCategory("Routing/InterfaceBased")]
	public class RoutingInterfaceHandlerTests
	{




		[TestMethod]
		public void AsyncInterfaceBasedRoutingShouldWork()
		{
			var processor = Substitute.For<IMessageHandlerAsync<SomeDto>, IMessageHandler<SomeDto>>();

			var router = RoutingTestEnvironment.GetInterfaceBasedRoutingProvider(processor);
			var routedObject = RoutingTestEnvironment.GetTestDtoData();
			router.RouteObject(null, routedObject);
			processor.Received().HandleMessage(routedObject);
		}




		[TestMethod]
		public void SimpleInterfaceBasedRoutingShouldWork()
		{
			var processor = Substitute.For<IMessageHandler<SomeDto>>();
			var router = RoutingTestEnvironment.GetInterfaceBasedRoutingProvider(processor);
			var routedObject = RoutingTestEnvironment.GetTestDtoData();
			router.RouteObject(null, routedObject);
			processor.Received().HandleMessage(routedObject);
		}
	}
}