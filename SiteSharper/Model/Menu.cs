using System.Collections.Generic;

namespace SiteSharper.Model
{
	public sealed class Menu
	{
		public List<IPageRef> Pages = new List<IPageRef>();

		public Menu page(IPageRef page)
		{
			Pages.Add(page);
			return this;
		}
	}
}
