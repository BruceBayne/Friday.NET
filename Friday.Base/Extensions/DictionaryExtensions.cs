﻿using System.Collections.Generic;
using System.Linq;

namespace Friday.Base.Extensions
{
	public static class DictionaryExtensions
	{
		public static void RemoveAllByValue<K, V>(this Dictionary<K, V> dictionary, V value)
		{
			foreach (var key in dictionary.Where(
				kvp => EqualityComparer<V>.Default.Equals(kvp.Value, value)).Select(x => x.Key).ToArray())
				dictionary.Remove(key);
		}
	}
}