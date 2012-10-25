using System;
using System.IO;
using SiteSharper.Reader;

namespace SiteSharper.Readers
{
	public sealed class JournalEntry
	{
		public DateTime Date;
		public string Content;

		public static JournalEntry fromFile(string filePath)
		{
			var date = DateReader.fromFilename(Path.GetFileName(filePath));
			var content = MarkdownReader.fromFile(filePath);
			return new JournalEntry{ Date = date, Content = content};
		}
	}
}