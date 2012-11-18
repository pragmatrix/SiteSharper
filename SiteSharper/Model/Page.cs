using SiteSharper.Reader;
using SiteSharper.Writer;

namespace SiteSharper.Model
{
	public class Page : IPageRef
	{
		public string Id { get; private set; }
		public string Name { get; private set; }
		public string ContentFilename = string.Empty;

		public Page(string id, string name)
		{
			Id = id;
			Name = name;
		}

		public Page sourceFile(string filename)
		{
			ContentFilename = filename;
			return this;
		}

		public virtual string Content
		{
			get
			{
				if (ContentFilename == string.Empty)
					return string.Empty;

				return MarkdownReader.fromFile(ContentFilename);
			}
		}

		internal virtual void writePage(PageWriter writer, string html)
		{
			writer.writePage(html);
		}

		public override string ToString()
		{
			return Id;
		}
	}
}
