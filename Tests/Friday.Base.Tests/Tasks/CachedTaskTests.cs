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
	class Suslik
	{
		public event EventHandler<int> SuslikRigaet;

		public void Rigat()
		{
			SuslikRigaet?.Invoke(this, 0);
		}
	}

	public class ValuePointer<TParent, TType>
	{
		private readonly TParent _instance;
		private readonly Func<TParent, TType> _propertyExpression;
		private readonly PropertyInfo _propInfo;
		private readonly FieldInfo _fieldInfo;

		public ValuePointer(TParent instance,
			Expression<Func<TParent, TType>> propertyExpression)
		{
			_instance = instance;
			_propertyExpression = propertyExpression.Compile();
			_propInfo = ((MemberExpression)(propertyExpression).Body).Member as PropertyInfo;
			_fieldInfo = ((MemberExpression)(propertyExpression).Body).Member as FieldInfo;
		}

		public TType Value
		{
			get { return _propertyExpression.Invoke(_instance); }
			set
			{
				if (_fieldInfo != null)
				{
					_fieldInfo.SetValue(_instance, value);
					return;
				}

				_propInfo.SetValue(_instance, value, null);
			}
		}
	}




	[TestClass]
	[TestCategory("Task-Extensions")]
	public class CachedTaskTests
	{
		[TestMethod]
		public void SuslikRigaet()
		{
			var susel = new Suslik();

			susel.SuslikRigaet += (sender, i) =>
			{
				int x = i;
			};


			System.Diagnostics.Trace.Listeners.Add(
				new System.Diagnostics.TextWriterTraceListener(Console.Out));
			System.Diagnostics.Trace.WriteLine("Error in Widget 42");

			//ed.Dispose();

			susel.Rigat();
		}


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