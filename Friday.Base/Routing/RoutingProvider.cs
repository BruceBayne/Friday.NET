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
			if (routeRule.RouteProcessor == null)
				throw new ArgumentNullException(nameof(routeRule.RouteProcessor));

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
				if (route.RouteProcessor is T result)
					yield return result;
			}
		}


		public void RouteObject(object objectForRoute)
		{
			var task = RouteObjectAsync(null, objectForRoute);
			task.Wait();
		}


		public void RouteObject(object context, object routedObject)
		{
			var task = RouteObjectAsync(context, routedObject);
			task.Wait();
		}


		public async Task RouteObjectAsync(object routedObject)
		{
			await RouteObjectAsync(null, routedObject);
		}


		public async Task RouteObjectAsync(object context, object routedObject)
		{
			var routingTable = GetSuitableRoutingRecords(context, routedObject);
			foreach (var record in routingTable)
			{
				var p = new ObjectToRoute(context, routedObject, record);
				await CallMethod(p);
			}
		}

		private IEnumerable<StaticRoutingTableRecord> GetSuitableRoutingRecords(object context, object routedObject)
		{
			var routingTable = GetStaticRoutingRecords(routedObject);

			if (HasMessageHandlerRouting)
				routingTable = routingTable.Concat(GetInterfaceRoutingRecords(context, routedObject));


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


		private IEnumerable<StaticRoutingTableRecord> GetInterfaceRoutingRecords(object context, object routedObject)
		{
			var interfaceMethodName = nameof(IMessageHandler<object>.HandleMessage);
			foreach (var apiRoute in routes.Where(t => t.Options.UseInterfaceMessageRouting))
			{
				var processorType = apiRoute.RouteProcessor.GetType();


				var allMessageHandlerTypes = processorType
					.GetInterfaces()
					.Where(TypeIsMessageHandler)
					.Where(x => TypeHandlerCompatibleTo(x, context, routedObject))
					.ToList();


				foreach (var compatibleTypes in allMessageHandlerTypes)
				{
					var methodInfo = compatibleTypes.GetMethod(interfaceMethodName);

					if (methodInfo != null)
						yield return new StaticRoutingTableRecord(apiRoute.RouteProcessor, methodInfo);
				}
			}
		}



		private static bool IsTypeCompatibleTo(Type t, params object[] x)
		{
			return x.Any(t.IsInstanceOfType);
		}

		private static bool TypeHandlerCompatibleTo(Type x, object context, object routedObject)
		{

			var genericArgCount = x.GetGenericArguments().Length;

			if (genericArgCount != 1 && genericArgCount != 2)
				return false;

			var firstGenericType = x.GetGenericArguments()[0];


			switch (genericArgCount)
			{
				case 1:
					return IsTypeCompatibleTo(firstGenericType, routedObject);
				case 2:
					var secondGenericType = x.GetGenericArguments()[1];

					var isCompatibleContextMessageHandler =
						IsTypeCompatibleTo(firstGenericType, context) &&
						IsTypeCompatibleTo(secondGenericType, routedObject);
					return isCompatibleContextMessageHandler;
			}

			return false;
		}

		private static bool TypeIsMessageHandler(Type x)
		{
			return x.IsGenericType &&
				   (
					   x.GetGenericTypeDefinition() == typeof(IMessageHandler<>) ||
					   x.GetGenericTypeDefinition() == typeof(IMessageHandler<,>) ||
					   x.GetGenericTypeDefinition() == typeof(IMessageHandlerAsync<>));
		}


		private IEnumerable<StaticRoutingTableRecord> GetStaticRoutingRecords(object routedObject)
		{
			foreach (var apiRoute in routes.Where(t => t.Options.UseStaticNameRouting))
			{
				var processorType = apiRoute.RouteProcessor.GetType();

				var methodName = ExtractMethodName(apiRoute, routedObject);

				var methodInfo = processorType.GetMethod(methodName, BindingFlags.Public | BindingFlags.Instance);
				if (methodInfo == null)
					continue;

				yield return new StaticRoutingTableRecord(apiRoute.RouteProcessor, methodInfo);

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