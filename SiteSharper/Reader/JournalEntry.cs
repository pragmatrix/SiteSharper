using System;
using System.IO;
using SiteSharper.Reader;

namespace SiteSharper.Readers
{
	public sealed class JournalEntry
	{
		public JournalEntryFilename Filename;
		public string Content;

		public static JournalEntry fromFile(string filePath)
		{
			var filename = JournalEntryFilename.fromFilename(Path.GetFileName(filePath));
			var content = MarkdownReader.fromFile(filePath);
			return new JournalEntry{ Filename = filename, Content = content};
		}
	}
}