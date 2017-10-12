namespace Friday.Base.Logging
{
    public class H1LoggerFormatter : BaseLogger
    {
        public H1LoggerFormatter(ILogger nestedLogger) : base(nestedLogger)
        {

        }

        public override void Log(LogLevel level, string message)
        {
            base.Log(level, "<h1>" + message + "</h1>");
        }
    }
}
