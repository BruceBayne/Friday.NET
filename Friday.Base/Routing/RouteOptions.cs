namespace Friday.Base.Routing
{
	public struct RouteOptions
	{
		public RouteProcessingBehavior ProcessingBehavior;

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


}