using System;
using System.Globalization;
using Toolbox;

namespace SiteSharper.Reader
{
	static class DateReader
	{
		public static string printDateTimeCode(string code)
		{
			DateTime date = fromDateTimeCode(code);

			switch (code.Length)
			{
				case 8:
				case 10:
				case 12:
					return date.ToString("MMMM dd, yyyy", CultureInfo.InvariantCulture);
			}

			return "";
		}

		public static DateTime fromDateTimeCode(string code)
		{
			switch (code.Length)
			{
			case 8:
			case 10:
			case 12:
				break;

			default:
				throw new Exception("Expect date time code to contain 8, 10 or 12 characters: {0}".format(code));
			}

			var year = 0;
			var month = 0;
			var day = 0;
			var hour = 0;
			var minute = 0;

			if (code.Length >= 8)
			{
				year = int.Parse(code.Substring(0, 4));
				month = int.Parse(code.Substring(4, 2));
				day = int.Parse(code.Substring(6, 2));
			}

			if (code.Length >= 10)
			{
				hour = int.Parse(code.Substring(8, 2));
			}

			if (code.Length >= 12)
			{
				minute = int.Parse(code.Substring(10, 2));
			}

			return new DateTime(year, month, day, hour, minute, 0);
		}
	}
}