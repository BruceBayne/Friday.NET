using Friday.Base.Routing.Interfaces;
using Friday.Base.Tests.Routing.Environment;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace Friday.Base.Tests.Routing
{
	[TestClass]
	public class RoutingDoubleRouteTests
	{
		public interface IXProcessor : IMessageHandler<string>
		{
			void OnString(string s);
		}

		[TestMethod]
		public void StaticWithInterfaceRoutingBothShouldWorkInOrder()
		{
			var xProcessor = Substitute.For<IXProcessor>();

			var router = RoutingTestEnvironment.GetStaticRoutingWithInterfaceRoutingProvider(xProcessor);
			router.RouteObject(string.Empty);
			xProcessor.Received(1).OnString(string.Empty);
			xProcessor.Received(1).HandleMessage(string.Empty);


		}


	}
}