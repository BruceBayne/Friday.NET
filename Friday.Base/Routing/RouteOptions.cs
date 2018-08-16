using System;

namespace Friday.Base.Routing
{


	public struct RouteOptions : IEquatable<RouteOptions>
	{
		public RouteProcessingBehavior ProcessingBehavior { get; private set; }
		public string RouteTemplate { get; private set; }
		public RouteCallType RouteCallType { get; private set; }

		public const string DefaultRoutingStaticTemplate = "On{typeName}";


		public static RouteOptions UseStaticDefaultTemplate()
		{
			return new RouteOptions(DefaultRoutingStaticTemplate);
		}


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
				RouteTemplate = String.Empty,
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

		public bool Equals(RouteOptions other)
		{
			return ProcessingBehavior == other.ProcessingBehavior && string.Equals(RouteTemplate, other.RouteTemplate) && RouteCallType == other.RouteCallType;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			return obj is RouteOptions && Equals((RouteOptions)obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				var hashCode = (int)ProcessingBehavior;
				hashCode = (hashCode * 397) ^ (RouteTemplate != null ? RouteTemplate.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (int)RouteCallType;
				return hashCode;
			}
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