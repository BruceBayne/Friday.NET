using System;
using System.Threading;
using System.Threading.Tasks;
using Friday.Base;
using Friday.Base.Logging;

namespace Friday.Service.Basics.Console
{
	public abstract class ConsoleTitleUpdater : IStartableMarker, IDisposable
	{
		private readonly ILogger logger;


		public abstract string GetServiceName();

		//  private readonly IRepositoryName repositoryName;
		private readonly CancellationTokenSource token = new CancellationTokenSource();
		private const int PredefinedWidth = 220;
		private const int PredefinedHeight = 1200;


		public ConsoleTitleUpdater(ILogger logger)
		{
			this.logger = logger;
		}

		private void UpdateTitle()
		{
			while (!token.IsCancellationRequested)
			{
				Colorful.Console.Title =
					$"Market.data.service: Uptime / {Uptime.FullUptime}";
				Thread.Sleep(TimeSpan.FromSeconds(1));
			}
		}

		public void Start()
		{
			Colorful.Console.SetBufferSize(PredefinedWidth, PredefinedHeight);
			logger.LogInformation($"{GetServiceName()} ready to serve");
			Task.Run(() => UpdateTitle());
		}

		public void Dispose()
		{
			token.Cancel();
		}
	}
}