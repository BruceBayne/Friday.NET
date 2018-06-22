using System;
using System.Threading.Tasks;

namespace Friday.Service.Basics
{
	internal class Some
	{

		public void Foo()
		{

			TaskScheduler.UnobservedTaskException += TaskSchedulerOnUnobservedTaskException;
			AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
			AppDomain.CurrentDomain.FirstChanceException += (sender, eventArgs) => { };

		}

		private void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs e)
		{

		}

		private void TaskSchedulerOnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
		{

		}
	}
}
