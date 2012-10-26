using SiteSharper.Model;

namespace SiteSharper
{
	public sealed class PageWriter
	{
		internal PageWriter(SiteWriter siteWriter, Page page)
		{
			SiteWriter = siteWriter;
			Page = page;
		}

		public Site Site { get { return SiteWriter.Site; } }
		public SiteWriter SiteWriter { get; private set; }
		public readonly Page Page;

		public void writePage(string html)
		{
			html = SiteWriter.postProcess(this, html);
			SiteWriter.writePage(Page, html);
		}
	}
}
