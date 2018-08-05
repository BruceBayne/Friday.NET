using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Friday.Base.Extensions.Assembly
{
	public static class AssemblyExtensions
	{
		public static DateTime GetLinkerDateTime(this System.Reflection.Assembly assembly)
		{
			return RetrieveLinkerTimestamp(assembly.Location);

		}


		/// <summary>
		/// Retrieves the linker timestamp.
		/// </summary>
		/// <param name="filePath">The file path.</param>
		/// <returns></returns>
		/// <remarks>http://www.codinghorror.com/blog/2005/04/determining-build-date-the-hard-way.html</remarks>
		private static DateTime RetrieveLinkerTimestamp(string filePath)
		{
			const int peHeaderOffset = 60;
			const int linkerTimestampOffset = 8;
			var b = new byte[2048];

			using (var ss = new FileStream(filePath, FileMode.Open, FileAccess.Read))
			{
				ss.Read(b, 0, 2048);
			}


			var secondsSince1970 = BitConverter.ToInt32(b, BitConverter.ToInt32(b, peHeaderOffset) + linkerTimestampOffset);

			var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			var linkTimeUtc = epoch.AddSeconds(secondsSince1970);
			var dt = TimeZoneInfo.ConvertTimeFromUtc(linkTimeUtc, TimeZoneInfo.Local);
			return dt;

		}
	}
}
