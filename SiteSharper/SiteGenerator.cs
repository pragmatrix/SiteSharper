using System.IO;
using SiteSharper.Model;
using SiteSharper.TemplateGenerator;

namespace SiteSharper
{
	public sealed class SiteGenerator
	{
		readonly CompiledTemplate _template;

		public SiteGenerator()
		{
			_template = Template.compile<PageContext>(SiteTemplateFilename);
		}

		public void generate(Site site)
		{
			copyResources(site);
			generatePages(site);
		}

		void generatePages(Site site)
		{
			var context = SiteContext.create(site);
			foreach (var page in site.Pages)
			{
				if (page.URL_ != null)
					continue;

				generatePage(context, page);
			}
		}

		static void copyResources(Site site)
		{
			foreach (var resource in site.Resources)
				copyResource(resource);

			if (site.ShortcutIcon_ != null)
				copyResource(site.ShortcutIcon_);
		}

		static void copyResource(string resource)
		{
			var fn = Path.GetFileName(resource);
			var targetPath = Path.Combine(SiteHtmlWriter.OutputDirectory, fn);
			File.Copy(resource, targetPath, true);
		}

		const string SiteTemplateFilename = SiteHtmlWriter.OutputDirectory + "/site.cshtml";
		
		void generatePage(SiteContext site, Page page)
		{
			var context = new PageContext(site, page);
			var html = _template.generateHTML(context);
			page.writePage(context, html);
		}
	}
}
	