using SiteSharper.Model;
using SiteSharper.Writer;

namespace SiteSharper
{
	public sealed class ModuleContext
	{
		public ModuleContext(PageWriter page, dynamic parameters)
		{
			_page = page;
			Parameters = parameters;
		}

		readonly PageWriter _page;
	
		public Site Site
		{
			get { return _page.Site; }
		}

		public SiteWriter SiteWriter
		{
			get { return _page.SiteWriter; }
		}

		public Page Page
		{
			get { return _page.Page; }
		}

		public readonly dynamic Parameters;
	}
}
