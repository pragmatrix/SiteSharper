using System;
using System.IO;
using System.Linq;
using ProductSite.Model;

namespace ProductSite
{
	public static class PageContentAutoResolver
	{
		public static Site loadContentFromDirectory(this Site site, string directory)
		{
			site.loadPageSourcesFromDirectory(directory);
			site.referResourcesFromDirectory(directory);
			return site;
		}

		static void loadPageSourcesFromDirectory(this Site site, string directory)
		{
			foreach (var page in site.Pages)
			{
				if (page.Content != string.Empty)
					continue;

				var filename = page.Id + ".md";
				var path = Path.Combine(directory, filename);
				if (!File.Exists(path))
					continue;

				page.sourceFile(path);
			}
		}

		static void referResourcesFromDirectory(this Site site, string directory)
		{
			foreach (var res in Directory.GetFiles(directory).Where(isResourceFilename))
			{
				site.resource(res);
			}
		}

		static bool isResourceFilename(string str)
		{
			var ext = Path.GetExtension(str).ToLowerInvariant();
			return -1 != Array.IndexOf(ResourceFileExtensions, ext);
		}

		static readonly string[] ResourceFileExtensions = new string[] { ".png", ".jpg"};
	}
}
