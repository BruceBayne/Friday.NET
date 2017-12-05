using System;
using FluentAssertions;
using Friday.Base.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
// ReSharper disable EventNeverSubscribedTo.Local
#pragma warning disable 67

namespace Friday.Base.Tests.Reflection
{
	[TestClass]
	public class EventUtilsTests
	{
		private class ClassWithEvents
		{
			public event EventHandler<string> OnString;
			public event EventHandler<int> OnInt;
		}

		[TestMethod]
		public void EventInvocationInThirdPartyClassShouldSuccess()
		{


			var e = new ClassWithEvents();
			var counter = 0;
			e.OnString += (sender, s) => counter++;
			e.OnString += (sender, s) => counter++;

			EventUtils.InvokeEvent(e, string.Empty);

			//Unfortunately FluentAssertions MonitorEvent/ShouldRaise Methods make this test fail
			counter.Should().Be(2);
		}
	}
}