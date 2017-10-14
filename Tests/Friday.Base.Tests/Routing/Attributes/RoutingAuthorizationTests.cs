using System;
using FluentAssertions;
using Friday.Base.Routing;
using Friday.Base.Routing.Attributes;
using Friday.Base.Tests.Routing.Environment;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace Friday.Base.Tests.Routing.Attributes
{
    [TestClass]
    [TestCategory("Routing")]
    public class RoutingAuthorizationTests
    {
        [TestMethod]
        public void CallOnAuthorizedRequiredMethodShouldGenerateNonAuthorizedException()
        {
            //Arrange
            var provider = RoutingTestEnvironment.GetAuthProvider();

            //Act
            Action a = () =>
                provider.RouteObject(Substitute.For<IRoutingContext>(), RoutingTestEnvironment.GetTestDtoData());

            //Assert
            a.ShouldThrow<AuthRequiredAttributeHandler.NotAuthorizedException>();
        }


        [TestMethod]
        public void AuthorizedMethodCallShouldWorkAfterAuthorization()
        {
            //Arrange
            var provider = RoutingTestEnvironment.GetAuthProvider();
            var routingContext = Substitute.For<IRoutingContext>();
            routingContext.Principal.Returns(info => null);


            //Act
            provider.RouteObject(routingContext, new AuthRequestDto());


            //Assert
            routingContext.Principal.Identity.IsAuthenticated.Should().BeTrue();
            routingContext.Principal.Should().NotBeNull();

            provider.RouteObject(routingContext, RoutingTestEnvironment.GetTestDtoData());
        }
    }
}