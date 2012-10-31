using SiteSharper.Reader;

namespace SiteSharper.Writer
{
	public sealed class RSSFeedWriter
	{
		public readonly JournalData Journal;
		public readonly string SiteDomainName;

		public string SitePath
		{
			get
			{
				return Journal.Journal.FeedSitePath; 
			}
		}

		public RSSFeedWriter(JournalData journal, string siteDomainName)
		{
			Journal = journal;
			SiteDomainName = siteDomainName;
		}
	}
}
