using System.Collections.Generic;

namespace Friday.Base.Random
{
    public interface IRandomCharGenerator
    {
        IEnumerable<char> Get();
        string GetSeed(int minCharCount, int maxCharCount);
    }
}