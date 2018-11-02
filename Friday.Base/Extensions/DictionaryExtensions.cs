using System;
using System.Collections.Generic;
using System.Linq;

namespace Friday.Base.Extensions
{
	public static class DictionaryExtensions
	{
		public static TValue FirstOrDefaultByValue<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TValue value)
		{
			foreach (var v in dictionary)
			{
				if (v.Value.Equals(value))
					return v.Value;
			}
			return default(TValue);
		}


		public static void RemoveAll<TKey, TValue>(this IDictionary<TKey, TValue> dic,
			Func<TKey, TValue, bool> predicate)
		{

			var keys = dic.Keys.Where(k => predicate(k, dic[k])).ToList();
			foreach (var key in keys)
			{
				dic.Remove(key);
			}
		}


		public static void RemoveAllByValue<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TValue value)
		{
			foreach (var key in dictionary.Where(
				kvp => EqualityComparer<TValue>.Default.Equals(kvp.Value, value)).Select(x => x.Key).ToArray())
				dictionary.Remove(key);
		}
	}
}