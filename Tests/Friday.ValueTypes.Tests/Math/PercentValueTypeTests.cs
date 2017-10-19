using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Friday.ValueTypes.Tests.Math
{
    [TestClass]
    [TestCategory("Percent")]
    public class PercentValueTypeTests
    {
        [TestMethod]
        public void BasicOperatorsOverrideShouldWork()
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

    }
}