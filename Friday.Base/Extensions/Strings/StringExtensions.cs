using System;
using System.Text.RegularExpressions;
using Friday.Base.Regexp;

namespace Friday.Base.Extensions.Strings
{
	public static class StringExtensions
	{



		public static string[] SplitCamelCase(this string source)
		{
			return Regex.Split(source, @"(?<!^)(?=[A-Z][a-z])");
		}


		public static bool IsValidEmail(this string input)
		{
			var regex = new Regex(RegularExpressions.EmailRegexPattern);
			return regex.IsMatch(input);
		}

		public static bool Contains(this string source, string value, StringComparison comp)
		{
			return source.IndexOf(value, comp) >= 0;
		}
		public static bool ContainsNoCase(this string source, string value)
		{
			return Contains(source, value, StringComparison.OrdinalIgnoreCase);
		}
	}
}