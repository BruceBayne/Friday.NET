using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Friday.ValueTypes.Tests.Math
{
    [TestClass]
    [TestCategory("Percent")]
    public class PercentValueTypeTests
    {
        [TestMethod]
        public void BasicComparisonOperatorsOverrideShouldWork()
        {
            var percent100 = Percent.Hundred;
            var percent100Again = Percent.Hundred;
            var percent50 = Percent.Fifty;
            percent100.Should().BeGreaterOrEqualTo(percent50);
            percent100.Should().BeGreaterOrEqualTo(percent100Again);
            percent100.Should().Equals(percent100Again);

            Assert.IsTrue(percent100 != percent50);
            Assert.IsTrue(percent100 == percent100Again);
            Assert.IsTrue(percent100 <= percent100Again);
            Assert.IsTrue(percent100 >= percent100Again);
            Assert.IsTrue(percent50 < percent100);
            Assert.IsTrue(percent50 <= percent100);
            Assert.IsTrue(percent100 >= percent50);
            Assert.IsTrue(percent100 > percent50);
        }

        [TestMethod]
        public void PercentToPercentAdditionAndSubtractionShouldWork()
        {
            var percent1 = Percent.Hundred;
            var percent2 = Percent.Fifty;
            (percent1 - percent2).Should().Be(Percent.Fifty);
            (percent1 + percent2).Should().Be(Percent.From(150));
        }

        [TestMethod]
        public void CalculateMethodShouldReturnExpectedResults()
        {
            Percent.CalculatePercentAmountFromValue(TestInt, Percent.Ten).Should().Be(TenPercentTestInt);
            Percent.CalculatePercentAmountFromValue(TestNegativeInt, Percent.Ten).Should().Be(-TenPercentTestInt);
            Percent.CalculatePercentAmountFromValue(TestLong, Percent.Ten).Should().Be(TenPercentTestInt);
            Percent.CalculatePercentAmountFromValue(TestNegativeLong, Percent.Ten).Should().Be(-TenPercentTestInt);
            Percent.CalculatePercentAmountFromValue(TestDecimal, Percent.Ten).Should().Be(TenPercentTestDecimal);
            Percent.CalculatePercentAmountFromValue(TestNegativeDecimal, Percent.Ten).Should().Be(-TenPercentTestDecimal);
        }

        [TestMethod]
        public void PercentToNumberAdditionAndSubtractionShouldWork()
        {
            (TestInt + Percent.Fifty).Should().Be(HundredFiftyPercentTestInt);
            (TestNegativeInt + Percent.Fifty).Should().Be(-HundredFiftyPercentTestInt);
            (TestInt - Percent.Fifty).Should().Be(FiftyPercentTestInt);
            (TestNegativeInt - Percent.Fifty).Should().Be(-FiftyPercentTestInt);

            (TestLong + Percent.Fifty).Should().Be(HundredFiftyPercentTestInt);
            (TestNegativeLong + Percent.Fifty).Should().Be(-HundredFiftyPercentTestInt);
            (TestLong - Percent.Fifty).Should().Be(FiftyPercentTestInt);
            (TestNegativeLong - Percent.Fifty).Should().Be(-FiftyPercentTestInt);


            (TestDecimal + Percent.Fifty).Should().Be(HundredFiftyPercentTestDecimal);
            (TestNegativeDecimal + Percent.Fifty).Should().Be(-HundredFiftyPercentTestDecimal);
            (TestDecimal - Percent.Fifty).Should().Be(FiftyPercentTestDecimal);
            (TestNegativeDecimal - Percent.Fifty).Should().Be(-FiftyPercentTestDecimal);
        }

        private int TestInt = 100500;
        private int TestNegativeInt = -100500;

        private long TestLong = 100500;
        private long TestNegativeLong = -100500;

        private decimal TestDecimal = 100.500M;
        private decimal TestNegativeDecimal = -100.500M;


        private int HundredFiftyPercentTestInt = 150750;
        private int FiftyPercentTestInt = 50250;
        private int TenPercentTestInt = 10050;

        private decimal TenPercentTestDecimal = 10.05M;
        private decimal HundredFiftyPercentTestDecimal = 150.75M;
        private decimal FiftyPercentTestDecimal = 50.25M;
    }
}