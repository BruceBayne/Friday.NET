using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Friday.Base.ValueTypes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Friday.ValueTypes.Tests.Serialization
{
	[TestClass]
	public class SerializationTests
	{
		[TestMethod]
		public void PercentShouldBeSerialized()
		{
			object percent = Percent.Fifty;
			percent.Should().BeBinarySerializable();
		}
	}
}