using System.IO;
using System.Linq;
using SiteSharper.Model;

namespace SiteSharper.Readers
{
	sealed class JournalData
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
	}
}
