using FluentAssertions;
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
            amount.ShouldBeEquivalentTo(BitCoin.One);
        }


        [TestMethod]
        public void OnePercentFromOneSatoshiShouldBeZero()
        {
            var amount = BitCoin.FromSatoshi(1).TakePercent(Percent.One);
            amount.ShouldBeEquivalentTo(BitCoin.Zero);
        }



        [TestMethod]
        public void OnePercentFromHundredSatoshiShouldBeOneSatoshi()
        {
            var amount = BitCoin.FromSatoshi(100).TakePercent(Percent.One);
            amount.ShouldBeEquivalentTo(BitCoin.FromSatoshi(1));
        }


        [TestMethod]
        public void FiftyPercentFromHundredSatoshiShouldBeFiftySasoshi()
        {
            var amount = BitCoin.FromSatoshi(100).TakePercent(Percent.Fifty);
            amount.ShouldBeEquivalentTo(BitCoin.FromSatoshi(50));
        }


        [TestMethod]
        public void BasicPercentageCalculationsShouldWorkProperly()
        {
            BitCoin.FromSatoshi(100).TakePercent(Percent.Fifty).ShouldBeEquivalentTo(BitCoin.FromSatoshi(50));
            BitCoin.FromSatoshi(100).TakePercent(Percent.Zero).ShouldBeEquivalentTo(BitCoin.FromSatoshi(0));

        }


    }
}