using System.IO;
using System.Web;

namespace SiteSharper.Reader
{
	public sealed class JournalEntryFilename
	{
		public string DateTimeCode;
		public string NamePart;

		public static JournalEntryFilename fromFilename(string filename)
		{
			var i = filename.IndexOf(" ", System.StringComparison.Ordinal);

			var dateTimeCode = "";
			var namePart = "";

			if (i == -1)
			{
				dateTimeCode = "";
				namePart = filename;
			}
			else
			{
				dateTimeCode = filename.Substring(0, i);
				namePart = Path.GetFileNameWithoutExtension(filename.Substring(i + 1));
			}

			return new JournalEntryFilename
			{
				DateTimeCode = dateTimeCode,
				NamePart = HttpUtility.UrlDecode(namePart)
			};
		}

		public override string ToString()
		{
			return DateTimeCode + " " + NamePart;
		}
	}
}
