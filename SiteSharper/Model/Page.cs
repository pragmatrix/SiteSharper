using System.IO;
using MarkdownSharp;
using SiteSharper.Reader;

namespace SiteSharper.Model
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
