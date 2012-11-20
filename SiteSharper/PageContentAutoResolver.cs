using System;
using System.IO;
using System.Linq;
using SiteSharper.Model;
using SiteSharper.Reader;

namespace SiteSharper
{
	public static class PageContentAutoResolver
	{
		public static Site loadContentFromDirectory(this Site site, string directory)
		{
			site.loadPageSourcesFromDirectory(directory);
			site.resources(directory);
			return site;
		}

		static void loadPageSourcesFromDirectory(this Site site, string directory)
		{
			foreach (var page in site.Pages)
			{
				if (page.Content != string.Empty)
					continue;

				var filename = page.getResourceFilePath() + ".md";
				var path = Path.Combine(directory, filename);
				if (!File.Exists(path))
					continue;

				var content = MarkdownReader.fromFile(path);
				page.Content = content;
			}
		}

		public static Site resources(this Site site, string sourceDirectoryPath, string targetDirectory = "")
		{
			foreach (var res in Directory.GetFiles(sourceDirectoryPath).Where(isResourceFilename))
			{
				var name = Path.GetFileName(res);
				site.resource(res, Path.Combine(targetDirectory, name));
			}
			return site;
		}

		static bool isResourceFilename(string str)
		{
			var ext = Path.GetExtension(str).ToLowerInvariant();
			return -1 != Array.IndexOf(ResourceFileExtensions, ext);
		}

		static readonly string[] ResourceFileExtensions = new string[] { ".png", ".jpg", ".gif", ".js", ".css"};
	}
}
