using System.IO;
using MarkdownSharp;

namespace SiteSharper.Reader
{
	static class MarkdownReader
	{
		public static string fromFile(string filename)
		{
			var text = File.ReadAllText(filename);
			return fromString(text);
		}

		public static string fromString(string text)
		{
			var md = new Markdown();
			return md.Transform(text);
		}
	}
}
