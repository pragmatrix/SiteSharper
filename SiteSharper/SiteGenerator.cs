using System.IO;
using System.Reflection;
using SiteSharper.Model;
using SiteSharper.TemplateGenerator;

namespace SiteSharper
{
	public sealed class SiteGenerator
	{
		readonly CompiledTemplate _template;
		readonly string _outputPath;

		public SiteGenerator(string outputPath)
		{
			_outputPath = outputPath;

			var siteSource = Path.Combine(AssemblyPath, "Site");
			_template = Template.compile<PageWriter>(Path.Combine(siteSource, SiteTemplateFilename));
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
			var context = SiteWriter.create(site, _outputPath);
			foreach (var page in site.Pages)
			{
				if (page.URL_ != null)
					continue;

				generatePage(context, page);
			}
		}

		void copyResources(Site site)
		{
			foreach (var resource in site.Resources)
				copyResource(resource);

			if (site.ShortcutIcon_ != null)
				copyResource(site.ShortcutIcon_);
		}

		void copyResource(string resource)
		{
			var fn = Path.GetFileName(resource);
			var targetPath = Path.Combine(_outputPath, fn);
			File.Copy(resource, targetPath, true);
		}

		void generatePage(SiteWriter site, Page page)
		{
			var context = new PageWriter(site, page);
			var html = _template.generateHTML(context);
			page.writePage(context, html);
		}
	}
}
	