using SiteSharper.Model;

namespace SiteSharper
{
	public sealed class ModuleContext
	{
		public ModuleContext(PageContext page, dynamic parameters)
		{
			_page = page;
			Parameters = parameters;
		}

		readonly PageContext _page;
	
		public Site Site
		{
			get { return _page.Site; }
		}

		public SiteContext SiteContext
		{
			get { return _page.SiteContext; }
		}

		public Page Page
		{
			get { return _page.Page; }
		}

		public readonly dynamic Parameters;
	}
}
