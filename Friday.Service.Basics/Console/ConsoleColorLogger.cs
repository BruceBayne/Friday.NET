using System;
using System.Drawing;
using Colorful;
using Friday.Base.Logging;
using Friday.Base.Regexp;


namespace Friday.Service.Basics.Console
{
	public class ConsoleColorLogger : BaseLogger
	{
		private readonly StyleSheet styleSheet;
		public const string HttpFtpUrlPattern = "(?:(?:(?:ftp)|(?:https?)):\\/\\/[\\w.\\-/]+)|(?:[\\w.-]+?.\\/[\\w.\\-%?]*)";


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

			FillupNumbers();
			FillupSuccess();
			FillupErrors();
			FillupDateTime();


			styleSheet.AddStyle("\\$", Color.LawnGreen);

			styleSheet.AddStyle("(?i)\\w*\\.*\\w*service*", Color.LawnGreen);
			styleSheet.AddStyle("(?i)\\w*uptime*", Color.CornflowerBlue);
			styleSheet.AddStyle("(?i)\\w*version*", Color.CornflowerBlue);


			styleSheet.AddStyle(HttpFtpUrlPattern, Color.SkyBlue);
			styleSheet.AddStyle("(?i)warn[ing]*", Color.Yellow);

			FillupCryptoPairs();




			styleSheet.AddStyle(RegularExpressions.IpV4AddressPattern, Color.AntiqueWhite);
		}

		private void FillupDateTime()
		{
			var timeRegex = "\\d\\d:\\d\\d:\\d\\d\\.*\\d*";
			var dateRegexp = "(\\d+)[-.\\/](\\d+)[-.\\/](\\d+)";
			styleSheet.AddStyle(timeRegex, Color.Pink);
			styleSheet.AddStyle(dateRegexp, Color.DarkTurquoise);
		}

		private void FillupCryptoPairs()
		{
			var slashSeparated = "\\w{1,3}?\\w?\\w\\w\\w\\/\\w?\\w?\\w\\w\\w";
			styleSheet.AddStyle(slashSeparated, Color.HotPink);
		}

		private void FillupErrors()
		{
			styleSheet.AddStyle("(?i)offline", Color.Red);
			styleSheet.AddStyle("(?i)exception", Color.Red);
		}

		private void FillupNumbers()
		{
			var integerNumberPattern = "\\s+[-]*\\(*\\d+\\)*[,:]*(\\s+|$)";
			styleSheet.AddStyle(integerNumberPattern, Color.Yellow);


			var floatNumberPattern = "\\s+[-]*\\(*\\d+[,\\.]\\d+\\)*[,:]*(\\s+|$)";
			styleSheet.AddStyle(floatNumberPattern, Color.Yellow);
		}

		private void FillupSuccess()
		{
			styleSheet.AddStyle("(?i)ready", Color.LightGreen);
			styleSheet.AddStyle("(?i)online", Color.LightGreen);
			styleSheet.AddStyle("(?i)connect[ed]*", Color.LightGreen);
			styleSheet.AddStyle("(?i)success[full]*", Color.LimeGreen);
		}

		public static string FormatLogLine2(string line)
		{
			var dtTime = DateTime.Now.ToString("hh:mm:ss.fff");
			return $"{dtTime} {line}";
		}

		public override void Log(LogLevel level, string message)
		{
			if (level == LogLevel.Error || level == LogLevel.Critical)
			{
				Colorful.Console.WriteLine(FormatLogLine2(message), Color.Red);
				return;
			}

			if (level == LogLevel.Warning)
			{
				Colorful.Console.WriteLine(FormatLogLine2(message), Color.Gold);
				return;
			}


			var formatMsg = FormatLogLine2($"{level} / {message}");
			Colorful.Console.WriteLineStyled(styleSheet, "{0}", formatMsg);
		}
	}
}