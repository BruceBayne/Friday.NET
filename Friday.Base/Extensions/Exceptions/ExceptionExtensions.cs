using System;

namespace Friday.Base.Extensions.Exceptions
{
    public static class ExceptionExtensions
    {


        public static Exception GetDeepestInnerException(this Exception e)
        {

            var innerException = e;
            while (innerException.InnerException != null)
                innerException = innerException.InnerException;
            return innerException;

        }



    }
}
