using Friday.Base.Routing;
using Friday.Base.Routing.Interfaces;
using Friday.Base.Tests.Routing.Environment;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace Friday.Base.Tests.Routing
{
	[TestClass]
	[TestCategory("Routing")]
	public class RoutingBasicTests
	{
		[TestMethod]
		public void SimplestStaticRoutingCallShouldWork()
		{
			//Arrange
			var processor = Substitute.For<ISomeApiProcessor>();
			var routingProvider = RoutingTestEnvironment.GetStaticNamesRoutingProvider(processor);
			var dto = RoutingTestEnvironment.GetTestDtoData();

			//Act
			routingProvider.RouteObject(new object(), dto);


			//Assert
			processor.Received().OnSomeDto(dto);
		}










		[TestMethod]
		public void RoutingObjectShouldBeForAllProcessorsByDefault()
		{

			//Arrange
			var processor = Substitute.For<ISomeApiProcessor>();
			var processor2 = Substitute.For<ISomeApiProcessor>();
			var routingProvider = RoutingTestEnvironment.GetStaticNamesRoutingProvider(processor, processor2);

			var dto = RoutingTestEnvironment.GetTestDtoData();

			//Act
			routingProvider.RouteObject(new object(), dto);

			//Assert
			processor.Received().OnSomeDto(dto);
			processor2.Received().OnSomeDto(dto);
		}

		[TestMethod]
		public void DirectInterfaceMethodCallShouldWork()
		{

			//Arrange
			var processor = Substitute.For<ISomeSessionProcessor>();
			var routingProvider = RoutingTestEnvironment.GetStaticNamesRoutingProvider(processor);

			//Act
			routingProvider.RouteCall<ISomeSessionProcessor>(
				sessionProcessor => { sessionProcessor.OnSessionClosed(); });

			//Assert
			processor.Received().OnSessionClosed();
		}



	}
}