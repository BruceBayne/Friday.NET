namespace Friday.Base.Routing
{
	public struct RouteRule
	{
		public readonly RouteOptions Options;

		public override string ToString()
		{
			return $"{nameof(RouteProcesor)}: {RouteProcesor.GetType().Name}, {nameof(Options)}: {Options}, {nameof(RouteTemplate)}: {RouteTemplate}";
		}

		public readonly string RouteTemplate;
		public readonly object RouteProcesor;
		public readonly string RouteName;

		public RouteRule(string routeName, object processor)
		{
			RouteTemplate = string.Empty;
			RouteProcesor = processor;
			Options = new RouteOptions();
			RouteName = routeName;

		}

		public RouteRule(string routeName, string routeTemplate, object processor)
		{
			RouteName = routeName;
			RouteTemplate = routeTemplate;
			RouteProcesor = processor;
			Options = new RouteOptions();
		}

		public RouteRule(string routeName, string routeTemplate, object processor, RouteOptions options)
		{
			RouteName = routeName;
			RouteTemplate = routeTemplate;
			RouteProcesor = processor;
			Options = options;
		}
	}
}