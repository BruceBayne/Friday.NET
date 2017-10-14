namespace Friday.Base.Routing
{
    public struct ObjectToRoute
    {
        public IRoutingContext Context;
        public object Payload;
        public StaticRoutingTableRecord RouteRecord;
    }
}