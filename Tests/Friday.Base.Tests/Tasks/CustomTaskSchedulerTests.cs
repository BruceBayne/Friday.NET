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

		public async Task FridayTaskSchedulerCountersShouldBeZeroAfterException2()
		{
			Assert.IsTrue(FridayTask.SetupFridaySchedulerAsDefault());


			var token = new CancellationTokenSource(TimeSpan.FromMilliseconds(500));

			var task = Task.Run(async () =>
			 {
				 await Task.Delay(TimeSpan.FromSeconds(5), token.Token);
			 }, token.Token);

			try
			{
				await task;
			}
			catch (TaskCanceledException e)
			{
				int x = 110;
			}
			catch (OperationCanceledException o)
			{
				int x = 1;
			}

			catch (Exception e)
			{
				Assert.Fail();
			}


			FridayTaskScheduler.EnqueuedTasks.Should().Be(0);
			FridayTaskScheduler.RunningTasks.Should().Be(0);
			FridayTask.TryRestoreDefaultTaskScheduler();
		}


		[TestMethod]

		public async Task FridayTaskSchedulerCountersShouldBeZeroAfterException()
		{
			Assert.IsTrue(FridayTask.SetupFridaySchedulerAsDefault());

			var task = Task.Run(async () =>
			{
				FridayTaskScheduler.EnqueuedTasks.Should().Be(1);
				await Task.Delay(50);
				throw new NotImplementedException();
			});

			try
			{
				await task;
				Assert.Fail("Task should throw");
			}
			catch (Exception)
			{
				// ignored
			}

			FridayTaskScheduler.EnqueuedTasks.Should().Be(0);
			FridayTaskScheduler.RunningTasks.Should().Be(0);
			FridayTask.TryRestoreDefaultTaskScheduler();
		}



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
			FridayTask.TryRestoreDefaultTaskScheduler();
		}
	}
}