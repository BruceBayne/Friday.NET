namespace Friday.Base.Routing
{
	public struct RouteRule
	{
		public readonly RouteOptions Options;

		public override string ToString()
		{
			return $"{nameof(RouteProcessor)}: {RouteProcessor.GetType().Name}, {nameof(Options)}: {Options}";
		}


		public static RouteRule UseInterfaceMessageHandler(object processor)
		{
			return new RouteRule(processor, RouteOptions.UseInterfaceMessageHandler());
		}

		public readonly object RouteProcessor;
		public readonly string RouteName;


		public RouteRule(object processor, RouteOptions options)
		{
			RouteName = processor.GetType().Name;
			RouteProcessor = processor;
			Options = options;
		}

		public RouteRule(object processor, RouteOptions options, string routeName)
		{
			RouteName = routeName;
			RouteProcessor = processor;
			Options = options;
		}
	}
}