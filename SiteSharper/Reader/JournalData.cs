using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using SiteSharper.Model;
using Toolbox;

namespace SiteSharper.Reader
{
	public sealed class JournalData
	{
		public Journal Journal;
		public JournalEntry[] Entries;

		public static JournalData read(Journal journal)
		{
			var entries = Directory.EnumerateFiles(journal.Path, "*.md")
				.Select(f => loadJournalEntry(journal, f))
				.OrderByDescending(entry => entry.Filename.ToString())
				.ToArray();

			return new JournalData {Journal = journal, Entries = entries};
		}

		static JournalEntry loadJournalEntry(Journal journal, string filePath)
		{
			var filename = JournalEntryFilename.fromFilename(Path.GetFileName(filePath));
			var content = MarkdownReader.fromFile(filePath);
			var entryId = journal.Id + "/" + ReadableURL.read(filename.ToString());

			var header = "[](module:BlogEntryHeader?entry={0}&name={1}&date={2})".format(
				HttpUtility.UrlEncode(entryId), 
				HttpUtility.UrlEncode(filename.NamePart),
				HttpUtility.UrlEncode(DateReader.printDateTimeCode(filename.DateTimeCode)));

			var headerHTML = MarkdownReader.fromString(header);

			return new JournalEntry { Id = entryId, Filename = filename, Content = headerHTML + content };
		}

		public IEnumerable<Page> createEntryPages()
		{
			return Entries.Select(createEntryPage);
		}

		Page createEntryPage(JournalEntry entry)
		{
			var filename = entry.Filename;
			var fileId = ReadableURL.read(filename.ToString());
			var pageId = Journal.Id + "/" + fileId;

			return new ContentPage(pageId, filename.NamePart, entry.Content);
		}
	}
}
