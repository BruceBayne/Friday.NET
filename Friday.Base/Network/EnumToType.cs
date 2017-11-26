using System;
using System.Collections.Generic;
using System.Linq;

namespace Friday.Base.Network
{
	public class EnumToType<TEnum, TBasicType> where TBasicType : IMessageType<TEnum>
	{
		private static readonly Dictionary<TEnum, Type> Cache = new Dictionary<TEnum, Type>();


		public Type GetType(TEnum e)
		{
			if (Cache.ContainsKey(e))
			{
				return Cache[e];
			}

			throw new ArgumentOutOfRangeException();
		}


		static EnumToType()
		{
			var types = typeof(TBasicType).Assembly.GetTypes()
				.Where(t => t.IsClass && !t.IsAbstract && typeof(TBasicType).IsAssignableFrom(t));

			foreach (var objext in types)
			{
				if (Activator.CreateInstance(objext) is IMessageType<TEnum> instance)
					Cache.Add(instance.MessageType, instance.GetType());
			}
		}
	}
}
