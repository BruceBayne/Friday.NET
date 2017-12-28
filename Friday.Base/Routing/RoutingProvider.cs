using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Friday.Base.Routing.Attributes;
using Friday.Base.Routing.Interfaces;

namespace Friday.Base.Routing
{
	public class RoutingProvider : IObjectRouter, IRouteRegistry
	{
		private readonly List<RouteAttributeHandler> attributeHandlers = new List<RouteAttributeHandler>();
		private readonly List<RouteRule> routes = new List<RouteRule>();

		private bool HasMessageHandlerRouting { get; set; }


		public IReadOnlyCollection<RouteRule> Routes => routes.ToList();

		public void RegisterAttributeHandler(RouteAttributeHandler handler)
		{
			attributeHandlers.Add(handler);
		}

		public void RegisterRoute(RouteRule routeRule)
		{
			if (routeRule.RouteProcesor == null)
				throw new ArgumentNullException(nameof(routeRule.RouteProcesor));

			if (routeRule.Options.UseInterfaceMessageRouting)
				HasMessageHandlerRouting = true;


			routes.Add(routeRule);
		}

		public bool UnregisterRoute(string routeName)
		{
			var isRemoved = routes.RemoveAll(x => x.RouteName == routeName) > 0;
			return isRemoved;
		}


		public void RouteCall<T>(Action<T> callAction) where T : class
		{
			var table = GetSuitableTableCallFor<T>();
			foreach (var route in table)
				callAction(route);
		}


		public async Task RouteCallAsync<T>(Func<T, Task> callAction) where T : class
		{
			var table = GetSuitableTableCallFor<T>();
			foreach (var route in table)
				await callAction(route);
		}

		private IEnumerable<T> GetSuitableTableCallFor<T>()
		{
			foreach (var route in routes)
			{
				if (route.RouteProcesor is T result)
					yield return result;
			}
		}




		public void RouteObject(object routedObject)
		{
			var task = RouteObjectAsync(null, routedObject);
			task.Wait();
		}



		public void RouteObject(IRoutingContext context, object routedObject)
		{
			var task = RouteObjectAsync(context, routedObject);
			task.Wait();
		}


		public async Task RouteObjectAsync(object routedObject)
		{
			await RouteObjectAsync(null, routedObject);
		}


		public async Task RouteObjectAsync(IRoutingContext context, object routedObject)
		{
			var routingTable = GetSuitableRoutingRecords(routedObject);
			foreach (var record in routingTable)
			{
				var p = new ObjectToRoute(context, routedObject, record);
				await CallMethod(p);
			}
		}

		private IEnumerable<StaticRoutingTableRecord> GetSuitableRoutingRecords(object routedObject)
		{
			var routingTable = GetStaticRoutingRecords(routedObject);

			if (HasMessageHandlerRouting)
				routingTable = routingTable.Concat(GetInterfaceRoutingRecords(routedObject));


			return routingTable;
		}


		private object[] ProvideMethodArguments(ObjectToRoute objectToRoute)
		{
			var methodParameters = objectToRoute.RouteRecord.SelectedMethod.GetParameters();

			var resultList = new List<object>(methodParameters.Length);

			foreach (var parameter in methodParameters)
			{
				if (parameter.ParameterType.IsInstanceOfType(objectToRoute.Context))
					resultList.Add(objectToRoute.Context);

				if (parameter.ParameterType.IsInstanceOfType(objectToRoute.Payload))
					resultList.Add(objectToRoute.Payload);

				if (parameter.ParameterType.IsInstanceOfType(this))
					resultList.Add(this);
			}

			return resultList.ToArray();
		}


		private async Task CallMethod(ObjectToRoute objectToRoute)
		{
			ProcessAttributeHandlers(objectToRoute);
			var routeRecord = objectToRoute.RouteRecord;
			var methodArguments = ProvideMethodArguments(objectToRoute);

			var result = routeRecord.SelectedMethod?.Invoke(routeRecord.Processor, methodArguments);
			if (result is Task t)
				await t;
		}

		private void ProcessAttributeHandlers(ObjectToRoute p)
		{
			foreach (var handler in attributeHandlers)
			{
				handler.Validate(p);
			}
		}


		private IEnumerable<StaticRoutingTableRecord> GetInterfaceRoutingRecords(object routedObject)
		{
			var interfaceMethodName = nameof(IMessageHandler<object>.HandleMessage);
			foreach (var apiRoute in routes.Where(t => t.Options.UseInterfaceMessageRouting))
			{
				var processorType = apiRoute.RouteProcesor.GetType();


				var allMessageHandlerTypes = processorType
					.GetInterfaces()
					.Where(x => x.IsGenericType &&

					(x.GetGenericTypeDefinition() == typeof(IMessageHandler<>) ||
					x.GetGenericTypeDefinition() == typeof(IMessageHandlerAsync<>))
					&& x.GetGenericArguments().First() == routedObject.GetType()
					).ToList();


				foreach (var compatibleTypes in allMessageHandlerTypes)
				{
					var methodInfo = compatibleTypes.GetMethod(interfaceMethodName);

					if (methodInfo != null)
						yield return new StaticRoutingTableRecord(apiRoute.RouteProcesor, methodInfo);
				}
			}
		}


		private IEnumerable<StaticRoutingTableRecord> GetStaticRoutingRecords(object routedObject)
		{
			foreach (var apiRoute in routes.Where(t => t.Options.UseStaticNameRouting))
			{
				var processorType = apiRoute.RouteProcesor.GetType();

				var methodName = ExtractMethodName(apiRoute, routedObject);

				var methodInfo = processorType.GetMethod(methodName, BindingFlags.Public | BindingFlags.Instance);
				if (methodInfo == null)
					continue;

				yield return new StaticRoutingTableRecord(apiRoute.RouteProcesor, methodInfo);

				if (apiRoute.Options.ProcessingBehavior == RouteProcessingBehavior.BreakAfterFirstMatch)
					yield break;
			}
		}

		private static string ExtractMethodName(RouteRule routeRule, object routedObject)
		{
			var methodName = routeRule.Options.RouteTemplate;
			methodName = methodName.Replace("{typeName}", routedObject.GetType().Name);
			return methodName;
		}
	}
}