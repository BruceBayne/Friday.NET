using System;

namespace Friday.Base.Random
{
    public static class FridayRandom
    {
        private static System.Random rnd = new System.Random(Environment.TickCount);
        public static bool IsRandomChanceOccured(int chanceInPercent)
        {
            if (chanceInPercent == 0)
                return false;
            var randomValue = GetNextInludingMax(1, 100);
            return randomValue <= chanceInPercent;
        }


        public static void SetSeed(int seed)
        {
            rnd = new System.Random(seed);
        }



        public static int GetNext() => GetNext(Int32.MinValue, int.MaxValue);

        /// <summary>
        /// This is own implementation  which include max in random range (e.g) min:1 max:5 can return 5
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns>Random from min to max including max</returns>
        public static int GetNextInludingMax(int min, int max)
        {
            return rnd.Next(min, max + 1);
        }



        /// <summary>
        /// This is standard GetNext for .NET Random which NOT including Max value in range (e.g) min:1 max:5 never return 5
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns>Random from min to max excluding max</returns>
        public static int GetNext(int min, int max)
        {
            return rnd.Next(min, max);
        }
    }




}