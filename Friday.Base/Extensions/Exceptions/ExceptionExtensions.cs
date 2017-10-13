using System;
using System.Reflection;
using System.Text;

namespace Friday.Base.Extensions.Exceptions
{
    public static class ExceptionExtensions
    {
        public static void PreserveStackTrace(this Exception exception)
        {
            MethodInfo preserveStackTrace = typeof(Exception).GetMethod(
                "InternalPreserveStackTrace",
                BindingFlags.Instance | BindingFlags.NonPublic);

            preserveStackTrace?.Invoke(exception, null);
        }

        public static string FlattenException(Exception exception)
        {
            var stringBuilder = new StringBuilder();

            while (exception != null)
            {
                stringBuilder.AppendLine(exception.Message);
                stringBuilder.AppendLine(exception.StackTrace);

                exception = exception.InnerException;
            }

            return stringBuilder.ToString();
        }

        public static Exception GetDeepestInnerException(this Exception e)
        {

            var innerException = e;
            while (innerException.InnerException != null)
                innerException = innerException.InnerException;
            return innerException;

        }



    }
}
