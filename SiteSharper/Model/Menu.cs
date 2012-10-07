using System.Collections.Generic;

namespace ProductSite.Model
{
	public sealed class Menu
	{
		public List<Page> Pages = new List<Page>();

		public Menu page(Page page)
		{
			Pages.Add(page);
			return this;
		}
	}
}
