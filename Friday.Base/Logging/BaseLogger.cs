using System;

namespace Friday.Base.Logging
{
    public abstract class BaseLogger : ILogger
    {
        private readonly ILogger innerLogger;

        protected virtual LogLevel MinLogLevel { get; set; } = LogLevel.Trace;

        protected BaseLogger()
        {
        }

        public static string FormatLogLine(string line)
        {
            var dtTime = DateTime.Now.ToString("hh.mm.ss.fff");
            return $"{dtTime} {line}";
        }

        protected virtual string ExceptionToString(Exception e)
        {
            return e.ToString();
        }

        public void SetMinimalLogLevel(LogLevel level)
        {
            LogWarning($"Logger is now on new TraceLevel: {level}");
            MinLogLevel = level;
        }


        protected BaseLogger(ILogger innerLogger)
        {
            this.innerLogger = innerLogger;
        }


        public void LogLine(LogLevel level, string line)
        {
            if (level < MinLogLevel || string.IsNullOrEmpty(line))
                return;
            Log(level, line);

        }

        public virtual void Log(LogLevel level, string message)
        {
            innerLogger?.LogLine(level, message);
        }

        public void Trace(string msg)
        {
            LogLine(LogLevel.Trace, msg);
        }

        public void LogDebug(string msg)
        {
            LogLine(LogLevel.Debug, msg);
        }

        public void LogInformation(string msg)
        {
            LogLine(LogLevel.Information, msg);
        }

        public void LogError(Exception e)
        {
            LogLine(LogLevel.Error, ExceptionToString(e));
        }

        public void LogError(string error)
        {
            LogLine(LogLevel.Error, error);
        }

        public void LogError(string error, Exception e)
        {
            LogLine(LogLevel.Error, error + Environment.NewLine + Environment.NewLine + ExceptionToString(e));
        }



        public void LogCritical(string error)
        {
            LogLine(LogLevel.Critical, error);
        }

        public void LogCritical(string e, Exception ex)
        {
            LogLine(LogLevel.Critical, e + Environment.NewLine + Environment.NewLine + ExceptionToString(ex));
        }

        public void LogCritical(Exception e)
        {
            LogLine(LogLevel.Critical, ExceptionToString(e));
        }

        public void LogWarning(string warning)
        {
            LogLine(LogLevel.Warning, warning);
        }
    }
}