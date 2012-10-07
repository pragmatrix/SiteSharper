using SiteSharper.Model;

namespace SiteSharper
{
	// must be public to be accessible from a compiled page template.
	public sealed class PageContext
	{
		internal PageContext(SiteContext siteContext, Page page)
		{
			SiteContext = siteContext;
			Page = page;
		}

		public Site Site { get { return SiteContext.Site; } }
		public SiteContext SiteContext { get; private set; }
		public readonly Page Page;


		public void writePage(string html)
		{
			html = SiteContext.postProcess(this, html);
			Site.writePage(Page, html);
		}
	}
}
