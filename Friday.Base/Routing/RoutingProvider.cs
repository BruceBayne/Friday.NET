using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Friday.Base.Routing.Attributes;

namespace Friday.Base.Routing
{
    public class RoutingProvider : IObjectRouter, IRouteRegistry
    {
        private readonly List<RouteAttributeHandler> attributeHandlers = new List<RouteAttributeHandler>();
        private readonly List<RouteRule> routes = new List<RouteRule>();

        public IReadOnlyCollection<RouteRule> Routes => routes.ToList();

        public void RegisterAttributeHandler(RouteAttributeHandler handler)
        {
            attributeHandlers.Add(handler);
        }

        public void RegisterRoute(RouteRule routeRule)
        {
            if (routeRule.RouteProcesor == null)
                throw new ArgumentNullException(nameof(routeRule.RouteProcesor));
            routes.Add(routeRule);
        }
        
        public bool UnregisterRoute(string routeName)
        {
            return routes.RemoveAll(x => x.RouteName == routeName)>0;
        }


        public void RouteCall<T>(Action<T> callAction) where T : class
        {
            var table = GetSuitableTableFor<T>();
            foreach (var route in table)
                callAction(route);
        }


        public async Task RouteCallAsync<T>(Func<T, Task> callAction) where T : class
        {
            var table = GetSuitableTableFor<T>();
            foreach (var route in table)
                await callAction(route);
        }

        private IEnumerable<T> GetSuitableTableFor<T>()
        {
            foreach (var route in routes)
            {
                if (route.RouteProcesor is T result)
                    yield return result;
            }
        }


        public void RouteObject(IRoutingContext context, object routedObject)
        {
            var task = RouteObjectAsync(context, routedObject);
            task.Wait();
        }


        public async Task RouteObjectAsync(IRoutingContext context, object routedObject)
        {
            var routingTable = GetRoutingRecordsFor(routedObject);

            foreach (var record in routingTable)
            {
                var p = new ObjectToRoute(context, routedObject, record);
                await CallMethod(p);
            }
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

        private IEnumerable<StaticRoutingTableRecord> GetRoutingRecordsFor(object routedObject)
        {
            foreach (var apiRoute in routes)
            {
                var processorType = apiRoute.RouteProcesor.GetType();

                var methodName = ExtractMethodName(apiRoute, routedObject);

                var methodInfo = processorType.GetMethod(methodName, BindingFlags.Public | BindingFlags.Instance);
                if (methodInfo == null)
                    continue;

                yield return new StaticRoutingTableRecord(apiRoute.RouteProcesor, methodInfo);

                if (apiRoute.Options.ProcessingBehavior == RouteProcessingBehavior.StopAfterFirstCall)
                    yield break;
            }
        }

        private static string ExtractMethodName(RouteRule routeRule, object routedObject)
        {
            var methodName = routeRule.RouteTemplate;
            methodName = methodName.Replace("{typeName}", routedObject.GetType().Name);
            return methodName;
        }
    }
}