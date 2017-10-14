namespace Friday.Base.Routing
{
    public struct RouteOptions
    {
        public RouteProcessingBehavior ProcessingBehavior;
    }

    public enum RouteProcessingBehavior
    {
        StopAfterFirstCall,
        ProcessAllRoutes,
    }


}