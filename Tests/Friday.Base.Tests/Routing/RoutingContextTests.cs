using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Friday.Base.Routing.Interfaces;
using Friday.Base.Tests.Routing.Environment;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Friday.Base.Tests.Routing
{




	[TestClass]
	public class RoutingContextTests
	{
		[TestMethod]
		public void AsyncHandlingWithContextShouldWork()
		{
			var processor = new SomeProcessor();
			var router = RoutingTestEnvironment.GetInterfaceBasedRoutingProvider(processor);

			Func<Task> f = async () => { await router.RouteObjectAsync(processor, string.Empty); };
			var throwResult = f.Should().Throw<AggregateException>();
			throwResult.WithInnerException<SomeRoutingException>();
		}
	}


	internal class SomeProcessor : IMessageHandlerAsync<SomeProcessor, string>
	{
		public Task HandleMessage(SomeProcessor context, string request)
		{
			throw new SomeRoutingException();
		}

	}
}