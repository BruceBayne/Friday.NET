using System;

namespace Friday.Base.Routing
{
	public struct RouteOptions
	{
		public RouteProcessingBehavior ProcessingBehavior { get; private set; }
		public string RouteTemplate { get; private set; }
		public RouteCallType RouteCallType { get; private set; }


		public static RouteOptions UseStatic(string routeTemplate)
		{
			return new RouteOptions(routeTemplate);
		}


		public static RouteOptions UseInterfaceMessageHandler(
			RouteProcessingBehavior behavior = RouteProcessingBehavior.Default)
		{
			var ro = new RouteOptions()
			{
				RouteCallType = RouteCallType.RouteAsInterfaceMessageHandler,
				RouteTemplate = string.Empty,
				ProcessingBehavior = behavior
			};
			return ro;
		}

		public RouteOptions(string routeTemplate,
			RouteProcessingBehavior processingBehavior = RouteProcessingBehavior.Default)
		{
			ProcessingBehavior = processingBehavior;
			RouteTemplate = routeTemplate;
			RouteCallType = RouteCallType.RouteAsStaticNameHandler;
		}


		public RouteOptions(RouteProcessingBehavior processingBehavior, string routeTemplate, RouteCallType callType)
		{
			ProcessingBehavior = processingBehavior;
			RouteTemplate = routeTemplate;
			RouteCallType = callType;
		}

		public bool UseInterfaceMessageRouting => RouteCallType.HasFlag(RouteCallType.RouteAsInterfaceMessageHandler);
		public bool UseStaticNameRouting => RouteCallType.HasFlag(RouteCallType.RouteAsStaticNameHandler);

		public override string ToString()
		{
			return $"{nameof(ProcessingBehavior)}: {ProcessingBehavior}";
		}
	}

	public enum RouteProcessingBehavior
	{
		Default,
		BreakAfterFirstMatch,
	}


	[Flags]
	public enum RouteCallType : byte
	{
		RouteAsStaticNameHandler = 1,
		RouteAsInterfaceMessageHandler = 2,
		RouteAsEventCallerHandler = 4,
		RouteAsAnySupported = 255,
	}
}