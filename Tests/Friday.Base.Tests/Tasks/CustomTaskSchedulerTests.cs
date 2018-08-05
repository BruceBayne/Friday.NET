using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Friday.Base.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Friday.Base.Tests.Tasks
{
	[TestClass]
	[TestCategory(nameof(CustomTaskSchedulerTests))]
	public class CustomTaskSchedulerTests
	{
		[TestMethod]
		public async Task FridayTaskSchedulerCountersShouldBeImpacted()
		{
			Assert.IsTrue(FridayTask.SetupFridaySchedulerAsDefault());


			var task = Task.Run(async () =>
			{
				FridayTaskScheduler.EnqueuedTasks.Should().Be(1);
				FridayTaskScheduler.RunningTasks.Should().Be(1);
				await Task.Delay(50);
			});

			await task;
			FridayTaskScheduler.RunningTasks.Should().Be(0);
			FridayTaskScheduler.EnqueuedTasks.Should().Be(0);
		}
	}
}