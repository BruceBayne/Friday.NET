namespace Friday.Base.Routing
{
	public struct RouteRule
	{
		public readonly RouteOptions Options;

		public override string ToString()
		{
			return $"{nameof(RouteProcesor)}: {RouteProcesor.GetType().Name}, {nameof(Options)}: {Options}";
		}


		public readonly object RouteProcesor;
		public readonly string RouteName;


		public RouteRule(object processor, RouteOptions options)
		{
			RouteName = processor.GetType().Name;
			RouteProcesor = processor;
			Options = options;
		}

		public RouteRule(object processor, RouteOptions options, string routeName)
		{
			RouteName = routeName;
			RouteProcesor = processor;
			Options = options;
		}
	}
}