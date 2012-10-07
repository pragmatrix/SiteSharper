using System.IO;
using MarkdownSharp;

namespace ProductSite.Model
{
	public class Page
	{
		public readonly string Id;
		public readonly string Name;
		public readonly string URL_;
		public string ContentFilename = string.Empty;

		public Page(string id, string name)
			: this(id, name, null)
		{
		}

		public Page(string id, string name, string url_)
		{
			Id = id;
			Name = name;
			URL_ = url_;
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
				
				var text = File.ReadAllText(ContentFilename);
				var md = new Markdown();
				return md.Transform(text);
			}
		}

		internal virtual void writePage(PageContext context, string html)
		{
			context.writePage(html);
		}
	}
}
