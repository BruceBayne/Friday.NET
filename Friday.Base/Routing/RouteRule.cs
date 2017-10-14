namespace Friday.Base.Routing
{
    public struct RouteRule
    {
        public readonly RouteOptions Options;
        public readonly string RouteTemplate;
        public readonly object RouteProcesor;


        public RouteRule(object processor)
        {
            RouteTemplate = string.Empty;
            RouteProcesor = processor;
            Options = RouteOptions.StopAfterFirstCall;
        }

        public RouteRule(string routeTemplate, object processor)
        {
            RouteTemplate = routeTemplate;
            RouteProcesor = processor;
            Options = RouteOptions.StopAfterFirstCall;
        }

        public RouteRule(string routeTemplate, object processor, RouteOptions options)
        {
            RouteTemplate = routeTemplate;
            RouteProcesor = processor;
            Options = options;
        }
    }
}