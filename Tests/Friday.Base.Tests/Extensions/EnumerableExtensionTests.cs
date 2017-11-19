using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Friday.Base.Extensions.Enumerable;
using Friday.Base.Random;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Friday.Base.Tests.Extensions
{
	[TestClass]
	[TestCategory("Friday.Base/Extensions")]
	public class EnumerableExtensionTests
	{

		[TestMethod]
		[DataTestMethod]
		[DataRow(10, 3)]
		[DataRow(11, 4)]
		public void TakeRandomShouldAlwaysReturnSameForSpecificSeed(int seed, int value)
		{
			FridayRandom.SetSeed(seed);
			var r = new List<int>() { 1, 2, 3, 4 };
			int v = r.TakeRandom();
			Assert.IsTrue(v == value);

		}


		[TestMethod]
		public void SymmetricExceptWithJaggedSequencesShouldReturnElement()
		{
			var x = new List<int> { 1, 2 };
			var y = new List<int> { 1, 2, 5, 4 };
			var i = x.SymmetricExcept(y).First();
			Assert.IsTrue(i == 5);
		}



		[TestMethod]
		public void SimpleChunkingShouldReturnPredefined()
		{
			var x = new List<int> { 1, 2, 3, 4, 5, 6, 7 };
			var chunkCount = x.TakeChunks(3).Count();
			chunkCount.Should().Be(3);

		}

	}
}
