using System.Collections.Generic;
using System.Linq;

namespace Friday.Base.Extensions.Enumerable
{
    public static partial class Enumerable
    {
        public static IEnumerable<char> Range(char firstLetter, char lastLetter)
        {
            return System.Linq.Enumerable.Range(firstLetter, lastLetter - firstLetter + 1).Select(c => (char)c).ToList();
        }
    }
}