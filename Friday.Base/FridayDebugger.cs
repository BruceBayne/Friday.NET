using System;
using System.Diagnostics;

namespace Friday.Base
{
    public static class FridayDebugger
    {

        public static void ConditionalBreak(Func<bool> breakCondition)
        {
            if (breakCondition())
                SafeBreak();
        }
        public static void SafeBreak()
        {

            if (Debugger.IsAttached)
                Debugger.Break();
        }

    }
}
