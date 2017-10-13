using System;
using System.Collections.Generic;
using System.Linq;
using Enumerable = Friday.Base.Extensions.Enumerable.Enumerable;


namespace Friday.Base.Random
{
    public class RandomCharGenerator : IRandomCharGenerator
    {
        private readonly RandomCharGeneratorConfiguration cfg;
        private readonly IEnumerable<char> alphaRange = Enumerable.Range('a', 'z');
        private readonly IEnumerable<char> capitalAlphaRange = Enumerable.Range('A', 'Z');
        private readonly IEnumerable<char> numericRange = Enumerable.Range('0', '9');
        private IEnumerable<char> workingRange = new char[0];
        private System.Random rng = new System.Random();
        public RandomCharGenerator(RandomCharGeneratorConfiguration cfg)
        {
            this.cfg = cfg;
            BuildWorkingRange();
        }

        public IEnumerable<char> Get()
        {
            while (true)
            {
                yield return workingRange.ElementAt(rng.Next(workingRange.Count()-1));
            }
        }

        public string GetSeed(int minCharCount, int maxCharCount)
        {
            return new string(Get().Take(rng.Next(minCharCount,maxCharCount)).ToArray());
        }

        private void BuildWorkingRange()
        {
            if(cfg.Alpha) workingRange = workingRange.Concat(alphaRange);
            if(cfg.CapitalAlpha) workingRange = workingRange.Concat(capitalAlphaRange);
            if(cfg.Numbers) workingRange = workingRange.Concat(numericRange);
            if(cfg.Custom) workingRange = workingRange.Concat(cfg.CustomRange);
            if(!workingRange.Any()) throw new Exception("No range specified");
        }
    }
}