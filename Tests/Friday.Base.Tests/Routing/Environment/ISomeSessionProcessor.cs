namespace Friday.Base.Tests.Routing.Environment
{
    public interface ISomeSessionProcessor : ISomeApiProcessor
    {
        void OnSessionClosed();
    }
}