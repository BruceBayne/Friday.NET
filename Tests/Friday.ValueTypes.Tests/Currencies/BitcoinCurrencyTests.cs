using FluentAssertions;
using Friday.ValueTypes.Currencies;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Friday.ValueTypes.Tests.Currencies
{
    [TestClass]
    [TestCategory(nameof(BitCoin))]
    public class BitcoinCurrencyTests
    {
        [TestMethod]
        public void TwoSameBitcoinAmountShouldBeEqual()
        {
            //Arrange
            var oneBtcValue = BitCoin.FromBtc(1);
            var anotherOneBtcValue = BitCoin.FromBtc(1);


            //Assert
            oneBtcValue.Should().Be(anotherOneBtcValue);
            Assert.IsTrue(oneBtcValue == anotherOneBtcValue);
        }


        [TestMethod]
        public void GreaterComparatorOfBitCoinShouldWorkProper()
        {
            var oneBtc = BitCoin.FromBtc(1);
            var twoBtc = BitCoin.FromBtc(2);


            twoBtc.Should().BeGreaterThan(oneBtc);
            Assert.IsTrue(twoBtc > oneBtc);
        }

        [TestMethod]
        public void LessComparatorOfBitCoinShouldWorkProper()
        {
            var oneBtc = BitCoin.FromBtc(1);
            var twoBtc = BitCoin.FromBtc(2);


            oneBtc.Should().BeLessThan(twoBtc);
            Assert.IsTrue(oneBtc < twoBtc);
        }


        [TestMethod]
        public void AddingTwoBitcoinsShouldBeCorrect()
        {
            var oneBtcValue = BitCoin.FromBtc(1);
            var sum = oneBtcValue + BitCoin.FromBtc(1);
            sum.Should().Be(BitCoin.FromBtc(2));
        }


        [TestMethod]
        public void OneBtcShouldBeMoreThan100Satoshi()
        {
            var oneBtcValue = BitCoin.FromBtc(1);
            var satoshiValue = BitCoin.FromSatoshi(100);

            oneBtcValue.Should().BeGreaterThan(satoshiValue);
        }
    }
}