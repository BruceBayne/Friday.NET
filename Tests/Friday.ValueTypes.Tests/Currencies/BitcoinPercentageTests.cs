using FluentAssertions;
using Friday.Base.ValueTypes;
using Friday.ValueTypes.Currencies;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Friday.ValueTypes.Tests.Currencies
{
	[TestClass]
	[TestCategory("BitCoin")]
	public class BitcoinPercentageTests
	{
		[TestMethod]
		public void HundredPercentFromSingleBitcoinShouldBeSingleBitcoin()
		{
			var amount = BitCoin.One.TakePercent(Percent.Hundred);
			amount.Should().BeEquivalentTo(BitCoin.One);
		}


		[TestMethod]
		public void OnePercentFromOneSatoshiShouldBeZero()
		{
			var amount = BitCoin.FromSatoshi(1).TakePercent(Percent.One);
			amount.Should().BeEquivalentTo(BitCoin.Zero);
		}


		[TestMethod]
		public void OnePercentFromHundredSatoshiShouldBeOneSatoshi()
		{
			var amount = BitCoin.FromSatoshi(100).TakePercent(Percent.One);
			amount.Should().BeEquivalentTo(BitCoin.FromSatoshi(1));
		}


		[TestMethod]
		public void FiftyPercentFromHundredSatoshiShouldBeFiftySasoshi()
		{
			var amount = BitCoin.FromSatoshi(100).TakePercent(Percent.Fifty);
			amount.Should().BeEquivalentTo(BitCoin.FromSatoshi(50));
		}


		[TestMethod]
		public void BasicPercentageCalculationsShouldWorkProperly()
		{
			BitCoin.FromSatoshi(100).TakePercent(Percent.Fifty).Should().BeEquivalentTo(BitCoin.FromSatoshi(50));
			BitCoin.FromSatoshi(100).TakePercent(Percent.Zero).Should().BeEquivalentTo(BitCoin.FromSatoshi(0));
		}
	}
}