using System.Collections.Generic;
using Friday.Base.Extensions.Enum;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Friday.Base.Tests.Extensions
{
    [TestClass]
    [TestCategory(nameof(EnumRandomTests))]
    public class EnumRandomTests
    {
        private enum TestEnum
        {
            Invalid,
            One,
            Two,
            Three,
            Four,
        }

        [TestMethod]
        public void EnsureOnlyAllowedRandomEnumValueBeGenerated()
        {
            var te = TestEnum.Invalid;
            Assert.IsTrue(te.RandomValue(new List<string>() { "One" }) == TestEnum.One);
        }


        [TestMethod]
        public void EnsureSpecificValueWillBeGeneratedUsingSeed()
        {
            var te = TestEnum.Invalid;
            var valueOneSeed = 3;
            Assert.IsTrue(te.RandomValue(new List<string>() { "One", "Two", "Three" }, valueOneSeed) == TestEnum.One);
        }


    }
}
