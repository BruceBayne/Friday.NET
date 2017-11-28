using System;
using System.Reflection;

namespace Friday.Base.Reflection
{
	public static class EventUtils
	{
		public const int EventArgsArgCount = 2;
		public const int EventArgsObjectParameter = 1;

		/// <summary>
		/// Search for EventHandler with type of 'eventArgument' in 'target' and Invoke that event 
		/// </summary>
		/// <param name="target">Target with EventHandler</param>
		/// <param name="eventArgument">Invoked Event Argument</param>
		public static void InvokeEvent(object target, object eventArgument)
		{
			if (eventArgument == null)
				throw new ArgumentNullException(nameof(eventArgument));

			var fields = target.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

			foreach (var field in fields)
			{
				var eventDelegate = field.GetValue(target) as Delegate;



				if (eventDelegate?.GetMethodInfo().GetParameters().Length != EventArgsArgCount)
					continue;


				var eventArgType = eventDelegate.GetMethodInfo().GetParameters()[EventArgsObjectParameter].ParameterType;

				if (eventArgType != eventArgument.GetType())
					continue;

				foreach (var eventHandler in eventDelegate.GetInvocationList())
					eventHandler.Method.Invoke(eventHandler.Target, new[] { target, eventArgument });
			}
		}
	}
}