	namespace Porrey.iWindow.Common
{
	public static class MagicValue
	{
		public static class Views
		{
			public const string StartPage = "Start";
			public const string MainPage = "Main";
		}

		public static class Property
		{
			public const string TemperatureUnit = "TemperatureUnit";
			public const string NtpServers = "NtpServers";
		}

		public static class TemperatureUnit
		{
			public const string Celcius = "C";
			public const string Fahrenheit = "F";
		}

		public static class Defaults
		{
			public const string TemperatureUnit = "C";
			public static string[] NtpServers = new string[] { "0.pool.np.org", "1.pool.np.org", "2.pool.np.org", "3.pool.np.org" };
		}

		public static class BackgroundService
		{
			public const string Timer = "Timer";
			public const string Ntp = "Ntp";
			public const string Keypad = "Keypad";
		}
	}
}