using System;
using System.Drawing;
using Colorful;
using Friday.Base.Logging;
using Friday.Base.Regexp;
using Console = Colorful.Console;


namespace Friday.Service.Basics
{
	public class ConsoleColorLogger : BaseLogger
	{
		private readonly StyleSheet styleSheet;

		public void ClearStyles()
		{
			styleSheet.Styles.Clear();
		}

		public void AddStyle(string regexp, Color color)
		{
			styleSheet.AddStyle(regexp, color);
		}


		public ConsoleColorLogger()
		{
			styleSheet = new StyleSheet(Color.BurlyWood);
			styleSheet.AddStyle("exception", Color.Red);

			styleSheet.AddStyle("\\s\\d+\\s", Color.Yellow);
			styleSheet.AddStyle("warn", Color.Gold);
			styleSheet.AddStyle("offline", Color.Red);
			styleSheet.AddStyle("online", Color.Green);
			styleSheet.AddStyle("connect", Color.Green);

			var slashSeparated = "\\w?\\w?\\w\\w\\w\\/\\w?\\w?\\w\\w\\w";
			styleSheet.AddStyle(slashSeparated, Color.HotPink);


			var timeRegex = "\\d\\d:\\d\\d:\\d\\d";
			var dateRegexp = "(\\d+)[-.\\/](\\d+)[-.\\/](\\d+)";
			styleSheet.AddStyle(timeRegex, Color.Pink);
			styleSheet.AddStyle(dateRegexp, Color.DarkTurquoise);


			styleSheet.AddStyle(RegularExpressions.IpV4AddressPattern, Color.AntiqueWhite);
		}


		public override void Log(LogLevel level, string message)
		{
			if (level == LogLevel.Error || level == LogLevel.Critical)
			{
				Console.WriteLine(FormatLogLine(message), Color.Red);
				return;
			}

			if (level == LogLevel.Warning)
			{
				Console.WriteLine(FormatLogLine(message), Color.Gold);
				return;
			}


			var formatMsg = FormatLogLine($"{level} / {message}");

			Console.WriteLineStyled(styleSheet, formatMsg);


			//base

			base.Log(level, message);
		}
	}
}