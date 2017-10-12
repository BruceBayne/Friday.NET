namespace Friday.Base.Logging
{
    public class LogSplitter : BaseLogger
    {
        private readonly ILogger nestedLogger;

        public LogSplitter(ILogger proxy, ILogger nestedLogger) : base(proxy)
        {
            this.nestedLogger = nestedLogger;
        }

        public override void Log(LogLevel level, string message)
        {
            nestedLogger.LogLine(level, message);
            base.Log(level, message);
        }
    }
}