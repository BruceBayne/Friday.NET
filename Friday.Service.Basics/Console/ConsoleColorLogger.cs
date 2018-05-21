using System;
using System.Drawing;
using Colorful;
using Friday.Base.Logging;
using Friday.Base.Regexp;


namespace Friday.Service.Basics.Console
{
	public class ConsoleColorLogger : BaseLogger
	{
		protected readonly StyleSheet StyleSheet;


		public void ClearStyles()
		{
			StyleSheet.Styles.Clear();
		}

		public void AddStyle(string regexp, Color color)
		{
			StyleSheet.AddStyle(regexp, color);
		}


		public ConsoleColorLogger()
		{

			StyleSheet = new StyleSheet(Color.BurlyWood);

			FillupNumbers();
			FillupSuccess();
			FillupErrors();
			FillupDateTime();


			StyleSheet.AddStyle("\\$", Color.LawnGreen);

			StyleSheet.AddStyle("(?i)\\w*\\.*\\w*service*", Color.LawnGreen);
			StyleSheet.AddStyle("(?i)\\w*uptime*", Color.CornflowerBlue);
			StyleSheet.AddStyle("(?i)\\w*version*", Color.CornflowerBlue);



			StyleSheet.AddStyle("(?i)warn[ing]*", Color.Yellow);

			FillupCryptoPairs();




			StyleSheet.AddStyle(RegularExpressions.IpV4AddressPattern, Color.AntiqueWhite);
		}

		private void FillupDateTime()
		{
			var timeRegex = "\\d\\d:\\d\\d:\\d\\d\\.*\\d*";
			var dateRegexp = "(\\d+)[-.\\/](\\d+)[-.\\/](\\d+)";
			StyleSheet.AddStyle(timeRegex, Color.Pink);
			StyleSheet.AddStyle(dateRegexp, Color.DarkTurquoise);
		}

		private void FillupCryptoPairs()
		{
			var slashSeparated = "\\w?\\w?\\w\\w\\w\\/\\w?\\w?\\w\\w\\w";
			StyleSheet.AddStyle(slashSeparated, Color.HotPink);
		}

		private void FillupErrors()
		{
			StyleSheet.AddStyle("(?i)offline", Color.Red);
			StyleSheet.AddStyle("(?i)exception", Color.Red);
		}

		private void FillupNumbers()
		{
			var integerNumberPattern = "\\s+[-]*\\(*\\d+\\)*[,:]*(\\s+|$)";
			StyleSheet.AddStyle(integerNumberPattern, Color.Yellow);


			var floatNumberPattern = "\\s+[-]*\\(*\\d+[,\\.]\\d+\\)*[,:]*(\\s+|$)";
			StyleSheet.AddStyle(floatNumberPattern, Color.Yellow);
		}

		private void FillupSuccess()
		{
			StyleSheet.AddStyle("(?i)ready", Color.LightGreen);
			StyleSheet.AddStyle("(?i)online", Color.LightGreen);
			StyleSheet.AddStyle("(?i)connect[ed]*", Color.LightGreen);
			StyleSheet.AddStyle("(?i)disconnect[ed]*", Color.Gold);
			StyleSheet.AddStyle("(?i)success[full]*", Color.LimeGreen);
		}

		public override void Log(LogLevel level, string message)
		{
			if (level == LogLevel.Error || level == LogLevel.Critical)
			{
				Colorful.Console.WriteLine(FormatLogLine(message), Color.Red);
				return;
			}

			if (level == LogLevel.Warning)
			{
				Colorful.Console.WriteLine(FormatLogLine(message), Color.Gold);
				return;
			}


			var formatMsg = FormatLogLine($"{level} / {message}");
			Colorful.Console.WriteLineStyled(formatMsg, StyleSheet);
		}
	}
}