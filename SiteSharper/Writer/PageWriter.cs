using SiteSharper.Model;

namespace SiteSharper.Writer
{
	public sealed class PageWriter
	{
		internal PageWriter(SiteWriter siteWriter, IPage page)
		{
			SiteWriter = siteWriter;
			Page = page;
		}

		public Site Site { get { return SiteWriter.Site; } }
		public SiteWriter SiteWriter { get; private set; }
		public readonly IPage Page;

		public void writePage(string html)
		{
			html = SiteWriter.postProcess(this, html);
			SiteWriter.writePage(Page, html);
		}
	}
}
