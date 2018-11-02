using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Friday.Base.Extensions.Assembly;
using Friday.Base.Tasks;
using Friday.Base.Tasks.CachedTask;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Friday.Base.Tests.Tasks
{


	[TestClass]
	[TestCategory("Task-Extensions")]
	public class CachedTaskTests
	{



		[TestMethod]
		public void TaskThatHasAttachedExceptionHandlers_ShouldNot_RaiseUnobservedTaskException()
		{
			TaskScheduler.UnobservedTaskException += (sender, args) =>
				Assert.Fail("This should never happens");

			for (int x = 0; x < 1000; x++)
			{
				var tc = new TaskCompletionSource<int>();
				tc.SetException(new Exception());
				tc.Task.AttachDefaultExceptionHandler();
			}

			GC.Collect();
			GC.WaitForFullGCComplete();
			GC.WaitForFullGCApproach();
			GC.WaitForPendingFinalizers();

		}


		[TestMethod]
		public async Task CachingShouldBeSuccess()
		{
			const int taskValue = 10;

			int counter = 0;

			var cachedTask = new CachedTask<int>(() =>
			{
				counter++;
				if (counter == 1)
					return Task.FromResult(taskValue);
				return Task.FromException<int>(new Exception());
			}, TimeSpan.FromMilliseconds(100), CacheFallbackPolicy.LeavePreviousSuccessValue);


			await cachedTask.GetResult();
			await cachedTask.GetResult();
			await cachedTask.GetResult();

			await Task.Delay(TimeSpan.FromSeconds(1)).ConfigureAwait(false);
			var x = await cachedTask.GetResult();
			x.Should().Be(taskValue);
		}
	}
}