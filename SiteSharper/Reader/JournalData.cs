using System.Collections.Generic;
using System.IO;
using System.Linq;
using SiteSharper.Model;
using SiteSharper.Reader;

namespace SiteSharper.Readers
{
	public sealed class JournalData
	{
		public Journal Journal;
		public JournalEntry[] Entries;

		public static JournalData read(Journal journal)
		{
			var entries = Directory.EnumerateFiles(journal.Path, "*.md")
				.Select(JournalEntry.fromFile)
				.ToArray();

			return new JournalData {Journal = journal, Entries = entries};
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
