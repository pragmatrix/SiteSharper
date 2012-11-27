using System.Collections.Generic;
using SiteSharper.Writer;

namespace SiteSharper.Model
{
	public class Page : IPage, ISpecifiesReferenceClasses
	{
		public string Id { get; private set; }
		public string Name { get; private set; }

		public List<string> ReferenceClasses { get; private set; }

		public Page(string id, string name)
		{
			Id = id;
			Name = name;
			Header = string.Empty;
			Content = string.Empty;
			ReferenceClasses = new List<string>();
		}

		public string Header { get; set; }

		public string Content { get; set; }

		public virtual void writePage(PageWriter writer, string html)
		{
			writer.writePage(html);
		}

		public override string ToString()
		{
			return Id;
		}
	}
}
