using System.IO;

namespace SiteSharper.Reader
{
	public sealed class JournalEntryFilename
	{
		public string DateTimeCode;
		public string NamePart;

		public static JournalEntryFilename fromFilename(string filename)
		{
			var i = filename.IndexOf(" ");
			return i == -1 
				? new JournalEntryFilename { DateTimeCode = "", NamePart = filename } 
				: new JournalEntryFilename {DateTimeCode = filename.Substring(0, i), NamePart = Path.GetFileNameWithoutExtension(filename.Substring(i + 1))};
		}

		public override string ToString()
		{
			return DateTimeCode + " " + NamePart;
		}
	}
}
