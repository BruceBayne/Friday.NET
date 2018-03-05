using System;
using System.Collections.Generic;
using System.Linq;
using Friday.Base.Extensions.Enumerable;


namespace Friday.Base.Random
{
	public class RandomCharGenerator : IRandomCharGenerator
	{
		private readonly RandomCharGeneratorConfiguration cfg;
		private readonly IEnumerable<char> alphaRange = FridayEnumerable.Range('a', 'z');
		private readonly IEnumerable<char> capitalAlphaRange = FridayEnumerable.Range('A', 'Z');
		private readonly IEnumerable<char> numericRange = FridayEnumerable.Range('0', '9');
		private IEnumerable<char> workingRange = new char[0];
		private readonly System.Random rng = new System.Random();
		public RandomCharGenerator(RandomCharGeneratorConfiguration cfg)
		{
			this.cfg = cfg;
			BuildWorkingRange();
		}

		public IEnumerable<char> Get()
		{
			while (true)
			{
				yield return workingRange.ElementAt(rng.Next(workingRange.Count() - 1));
			}
		}

		public string GetSeed(int minCharCount, int maxCharCount)
		{
			return new string(Get().Take(rng.Next(minCharCount, maxCharCount)).ToArray());
		}

		private void BuildWorkingRange()
		{
			if (cfg.Alpha) workingRange = workingRange.Concat(alphaRange);
			if (cfg.CapitalAlpha) workingRange = workingRange.Concat(capitalAlphaRange);
			if (cfg.Numbers) workingRange = workingRange.Concat(numericRange);
			if (cfg.Custom) workingRange = workingRange.Concat(cfg.CustomRange);
			if (!workingRange.Any()) throw new Exception("No range specified");
		}
	}
}