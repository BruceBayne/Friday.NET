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
		public void SimpleInterfaceRoutingWithContextShouldWork2()
		{
			var processor = Substitute.For<ComplexInterface>();
			var router = RoutingTestEnvironment.GetInterfaceBasedRoutingProvider(processor);
			var routedObject = RoutingTestEnvironment.GetTestDtoData();
			var context = new SomeContext();
			router.RouteObject(context, routedObject);

			processor.Received().HandleMessage(context, routedObject);
			processor.Received().HandleMessage(routedObject);


			processor.DidNotReceive().HandleMessage(routedObject, routedObject);
			processor.DidNotReceive().HandleMessage(routedObject, context);
			processor.DidNotReceive().HandleMessage(context, context);
		}








		[TestMethod]
		public void SimpleInterfaceRoutingWithContextShouldWork()
		{
			var processor = Substitute.For<IMessageHandler<SomeContext, SomeDto>>();
			var router = RoutingTestEnvironment.GetInterfaceBasedRoutingProvider(processor);
			var routedObject = RoutingTestEnvironment.GetTestDtoData();
			var context = new SomeContext();
			router.RouteObject(context, routedObject);
			processor.Received().HandleMessage(context, routedObject);
		}


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