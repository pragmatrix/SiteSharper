using System.IO;
using System.Reflection;
using SiteSharper.Model;
using SiteSharper.TemplateGenerator;

namespace SiteSharper
{
	public sealed class SiteGenerator
	{
		readonly CompiledTemplate _template;

		public SiteGenerator()
		{
			var siteSource = Path.Combine(AssemblyPath, "Site");
			_template = Template.compile<PageContext>(Path.Combine(siteSource, SiteTemplateFilename));
		}

		public static readonly string AssemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

		const string SiteTemplateFilename = "site.cshtml";

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

		
		void generatePage(SiteContext site, Page page)
		{
			var context = new PageContext(site, page);
			var html = _template.generateHTML(context);
			page.writePage(context, html);
		}
	}
}
	