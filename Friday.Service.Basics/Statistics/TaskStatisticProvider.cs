using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Tracing;

namespace Friday.Service.Basics.Statistics
{
	public sealed class TaskStatisticProvider : IDisposable
	{
		// http://referencesource.microsoft.com/#mscorlib/system/threading/Tasks/TPLETWProvider.cs,30
		private static readonly Guid TplEventSourceGuid = new Guid("2e5dba47-a3d2-4d16-8ee0-6671ffdcd7b5");

		// http://referencesource.microsoft.com/#mscorlib/system/threading/Tasks/TPLETWProvider.cs,130
		private const EventKeywords AsyncCausalityOperationKeyword = (EventKeywords)8;

		// http://referencesource.microsoft.com/#mscorlib/system/threading/Tasks/TPLETWProvider.cs,200
		private const int TRACEOPERATIONSTART_ID = 14;

		private const int TRACEOPERATIONSTOP_ID = 15;

		// http://referencesource.microsoft.com/#mscorlib/system/threading/Tasks/AsyncCausalityTracer.cs,42
		private const int AsyncCausalityStatusError = 3;

		private readonly EventListener eventListener = new EventListener();

		public IReadOnlyDictionary<int, string> AsyncMethodsInFlight => asyncMethodsInFlight;
		public IReadOnlyCollection<string> AsyncExceptions => asyncExceptions;

		public int ActiveTasks => asyncMethodsInFlight.Count;

		private readonly ConcurrentDictionary<int, string> asyncMethodsInFlight = new ConcurrentDictionary<int, string>();
		private readonly ConcurrentBag<string> asyncExceptions = new ConcurrentBag<string>();



		public TaskStatisticProvider()
		{
			eventListener.EventWritten += OnEventWritten;
			eventListener.EventSourceCreated += OnEventSourceCreated;

			// Cause the type initializer for System.Threading.Tasks.TplEtwProvider to run.
			// Otherwise async method builders starting events will be missed.
			Type.GetType("System.Threading.Tasks.TplEtwProvider, mscorlib", true).GetField("Log").GetValue(null);
		}

		public void Dispose()
		{
			eventListener.Dispose();
		}

		private static void OnEventSourceCreated(object sender, EventSourceCreatedEventArgs e)
		{
			if (e.EventSource.Guid != TplEventSourceGuid) return;
			var eventListener = (EventListener)sender;
			eventListener.EnableEvents(e.EventSource, EventLevel.Informational,
				AsyncCausalityOperationKeyword | (EventKeywords)0x20000);
		}

		private void OnEventWritten(object sender, EventWrittenEventArgs e)
		{
			switch (e.EventId)
			{
				case TRACEOPERATIONSTART_ID:
					{
						// http://referencesource.microsoft.com/#mscorlib/system/threading/Tasks/TPLETWProvider.cs,636
						var operationName = (string)e.Payload[1];

						// http://referencesource.microsoft.com/#mscorlib/system/runtime/compilerservices/AsyncMethodBuilder.cs,169
						var shouldTrack = operationName.StartsWith("Async: ");
						if (!shouldTrack) return;

						var taskId = (int)e.Payload[0];
						if (!asyncMethodsInFlight.TryAdd(taskId, operationName))
							throw new NotImplementedException("Task already associated with an in-flight async void method.");
						break;
					}
				case TRACEOPERATIONSTOP_ID:
					{
						// http://referencesource.microsoft.com/#mscorlib/system/threading/Tasks/TPLETWProvider.cs,674
						var taskId = (int)e.Payload[0];
						var status = (int)e.Payload[1];

						var isTracked = asyncMethodsInFlight.TryRemove(taskId, out var operationName);
						if (status == AsyncCausalityStatusError)
						{
							if (isTracked)
							{
								asyncExceptions.Add(operationName);
							}
							else
							{
								var trace = new StackTrace(skipFrames: 1);
								for (var i = 0; i < trace.FrameCount - 1; i++)
								{
									var method = trace.GetFrame(i).GetMethod();
									if (method == null || method.Name != "SetException") continue;
									if (method.DeclaringType.FullName != "System.Runtime.CompilerServices.AsyncVoidMethodBuilder") continue;
									asyncExceptions.Add("Async: " + trace.GetFrame(i + 1).GetMethod().DeclaringType.Name);
									break;
								}
							}
						}
						break;
					}
			}
		}
	}
}