using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Friday.Base.Extensions.Strings;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Friday.Base.Tests.Extensions
{

    [TestClass]
    [TestCategory(nameof(StringExtensionTests))]
    public class StringExtensionTests
    {

        [DataTestMethod]
        [DataRow("/:?", "___")]
        [DataRow("GoodFolder", "GoodFolder", DisplayName = "Good folder")]
        [DataRow("Invalid/Folder", "Invalid_Folder", DisplayName = "Invalid folder")]
        [DataRow("todo.txt", "todo.txt", DisplayName = "Good file name")]
        [DataRow("bad/name :)~$?%^.jpg", "bad_name _)~$_%^.jpg", DisplayName = "Invalid file name")]
        public void Invalid_Chars_In_Path_Should_Be_Replaced(string input, string output)
        {
            input.FixInvalidFileNameChars().Should().Be(output);
        }


    }
}
