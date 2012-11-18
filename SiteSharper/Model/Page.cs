using SiteSharper.Writer;

namespace SiteSharper.Model
{
	public class Page : IPage
	{
		public string Id { get; private set; }
		public string Name { get; private set; }

		public Page(string id, string name)
		{
			Id = id;
			Name = name;
			Content = string.Empty;
		}

		public string Header { get; private set; }

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
