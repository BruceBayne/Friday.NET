using System;
using System.Collections.Generic;

namespace Friday.Base.Reflection
{

	public sealed class TypeToAction : TypeToAction<object>
	{

	}


	public class TypeToAction<T>
	{
		private readonly Dictionary<Type, Action<T>> typeToAction =
			new Dictionary<Type, Action<T>>();

		public void RegisterAction<TType>(Action<TType> action) where TType : T
		{
			typeToAction.Add(typeof(TType), x => action?.Invoke((TType)x));
		}

		public bool TryCallByType(T message)
		{
			typeToAction.TryGetValue(message.GetType(), out var action);

			if (action == null)
				return false;

			action.Invoke(message);
			return true;
		}
	}
}