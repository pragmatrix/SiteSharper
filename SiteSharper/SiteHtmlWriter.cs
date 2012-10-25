using System.IO;
using SiteSharper.Model;

namespace SiteSharper
{
	static class SiteHtmlWriter
	{
		public static void writePage(this Site site, Page page, string html)
		{
			site.writeHTML(site.sitePathOf(page), html);
		}

		public static void writeHTML(this Site site, string relativePath, string html)
		{
			var outputPath = Path.Combine(OutputDirectory, relativePath);
			var outputDir = Path.GetDirectoryName(outputPath);
			Directory.CreateDirectory(outputDir);
			File.WriteAllText(outputPath, html);
		}

		public const string OutputDirectory = "Site";
	}
}
