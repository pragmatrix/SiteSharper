namespace SiteSharper.Reader
{
	public sealed class JournalEntry
	{
		public string Id;
		public JournalEntryFilename Filename;
		public string Content;

		public string Title
		{
			get { return Filename.NamePart; }
		}
	}
}