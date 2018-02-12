using System;
using System.Drawing;
using Colorful;
using Friday.Base.Logging;
using Console = Colorful.Console;


namespace Friday.Service.Basics
{
	public class ConsoleColorLogger : BaseLogger
	{
		protected virtual string FormatMsg(string msg)
		{
			var dtTime = DateTime.Now.ToString("hh:mm:ss.fff");
			return $"{dtTime} {msg}";
		}

		protected readonly StyleSheet styleSheet;

		public ConsoleColorLogger(StyleSheet styleSheet)
		{
			this.styleSheet = styleSheet;
		}


		public override void Log(LogLevel level, string message)
		{

			if (level == LogLevel.Trace)
				Console.WriteLineStyled(styleSheet, FormatMsg(message));

			if (level == LogLevel.Debug)
				Console.WriteLine(FormatMsg(message), Color.Gray);


			if (level == LogLevel.Information)
				Console.WriteLine(FormatMsg(message), Color.DodgerBlue);


			if (level == LogLevel.Error || level == LogLevel.Critical)
				Console.WriteLine(FormatMsg(message), Color.Red);

			if (level == LogLevel.Warning)
				Console.WriteLine(FormatMsg(message), Color.Gold);

			base.Log(level, message);

		}


	}
}