using System;
using System.Threading.Tasks;
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
		class Context : ISessionStarted
		{
			public void SessionStarted()
			{
			}
		}

		class MessageBase
		{
		}

		class MessageDerived : MessageBase
		{
		}

		interface ISessionStarted
		{
			void SessionStarted();
		}


		class Handler : ISessionStarted, IMessageHandler<ISessionStarted, MessageBase>, IMessageHandler<int>
		{
			public void SessionStarted()
			{
			}


			public void HandleMessage(ISessionStarted sender, MessageBase message)
			{
			}

			public void HandleMessage(int message)
			{

			}
		}








		[TestMethod]
		public void Foo()
		{
			var m = new MessageDerived();

			var rp = new RoutingProvider();
			rp.RegisterRoute(RouteRule.UseInterfaceMessageHandler(new Handler()));

			//rp.RouteCall<ISessionStarted>(x => x.SessionStarted());
			rp.RouteObject(m);
			rp.RouteObject(11);
			rp.RouteObject(11L);


			//var t = new List<string>();
			//List<object> l= (List<object>)t;
			//rp.RouteObject(new Context(), m);
		}


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