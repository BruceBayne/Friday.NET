using System.Collections.Generic;

namespace Friday.Base.Random
{
    public class RandomCharGeneratorConfiguration
    {
        public bool CapitalAlpha;
        public bool Alpha;
        public bool Numbers;
        public bool Custom;
        public IEnumerable<char> CustomRange;

        public static RandomCharGeneratorConfiguration Numeric()
        {
            return new RandomCharGeneratorConfiguration()
            {
                CapitalAlpha = false,
                Alpha = false,
                Numbers = true,
                Custom = false,
                CustomRange = string.Empty
            };
        }
        public static RandomCharGeneratorConfiguration AlphaNumeric()
        {
            return new RandomCharGeneratorConfiguration()
            {
                CapitalAlpha = true,
                Alpha = true,
                Numbers = true,
                Custom = false,
                CustomRange = string.Empty
            };
        }

        public static RandomCharGeneratorConfiguration CustomRangeOnly(string customRange)
        {
            return new RandomCharGeneratorConfiguration()
            {
                CapitalAlpha = false,
                Alpha = false,
                Numbers = false,
                Custom = true,
                CustomRange = customRange
            };
        }
    }
}