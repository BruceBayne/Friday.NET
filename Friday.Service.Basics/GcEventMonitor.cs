using System;
using System.Diagnostics;
using System.Runtime;
using System.Threading;
using System.Threading.Tasks;
using Friday.Base;

namespace Friday.Service.Basics
{
	public sealed class GcEventMonitor : IStartableMarker, IDisposable
	{
		public static bool IsServerGc => GCSettings.IsServerGC;

		private CancellationTokenSource cancellationToken;

		public event EventHandler<Stopwatch> OnGarbageCollectionFinished;

		private async Task WaitForGcEvents()
		{
			var sw = new Stopwatch();

			while (!cancellationToken.IsCancellationRequested)
			{
				var status = GC.WaitForFullGCApproach();

				if (status == GCNotificationStatus.Succeeded)
				{
					sw.Start();
					status = GC.WaitForFullGCComplete();
					sw.Stop();

					if (status == GCNotificationStatus.Succeeded)
					{
						DoGcFinished(sw);
					}
				}
				sw.Reset();
				await Task.Delay(TimeSpan.FromSeconds(1));
			}
		}

		private void DoGcFinished(Stopwatch e)
		{
			var handler = OnGarbageCollectionFinished;
			handler?.Invoke(null, e);
		}



		public void SetLatencyMode(GCLatencyMode newMode)
		{
			GCSettings.LatencyMode = newMode;
		}




		public void Start()
		{

			if (!IsReadyForStart())
				return;

			GC.RegisterForFullGCNotification(1, 1);
			Task.Factory.StartNew(WaitForGcEvents, TaskCreationOptions.LongRunning);
		}

		private bool IsReadyForStart()
		{
			return cancellationToken == null;
		}

		public void Dispose()
		{
			cancellationToken.Dispose();
		}
	}
}
