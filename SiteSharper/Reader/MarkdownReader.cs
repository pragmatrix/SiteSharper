using System.IO;
using MarkdownSharp;

namespace SiteSharper.Reader
{
	static class MarkdownReader
	{
		public static string fromFile(string filename)
		{
			var text = File.ReadAllText(filename);
			var md = new Markdown();
			return md.Transform(text);
		}
	}
}
