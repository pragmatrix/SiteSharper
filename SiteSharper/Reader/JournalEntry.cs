using System;
using SiteSharper.Reader;

namespace SiteSharper.Readers
{
	sealed class JournalEntry
	{
		public DateTime Date;
		public string Content;

		public static JournalEntry fromFile(string file)
		{
			var date = DateReader.fromFilename(file);
			var content = MarkdownReader.fromFile(file);
			return new JournalEntry{ Date = date, Content = content};
		}
	}
}