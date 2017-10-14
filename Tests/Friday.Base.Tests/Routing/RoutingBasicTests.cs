using Friday.Base.Routing;
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
        public void SimplestRoutingCallShouldWork()
        {
            var processor = Substitute.For<ISomeApiProcessor>();
            var routingProvider = RoutingTestEnvironment.GetDefaultRoutingProvider(processor);

            var dto = RoutingTestEnvironment.GetTestDtoData();
            routingProvider.RouteObject(Substitute.For<IRoutingContext>(), dto);

            processor.Received().OnSomeDto(dto);
        }


        [TestMethod]
        public void DirectInterfaceMethodCallShouldWork()
        {

            //Arrange
            var processor = Substitute.For<ISomeSessionProcessor>();
            var routingProvider = RoutingTestEnvironment.GetDefaultRoutingProvider(processor);
            
            //Act
            routingProvider.RouteCall<ISomeSessionProcessor>(
                sessionProcessor => { sessionProcessor.OnSessionClosed(); });
            
            //Assert
            processor.Received().OnSessionClosed();
        }
    }
}