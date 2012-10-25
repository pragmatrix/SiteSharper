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
			_template = Template.compile<PageWriter>(Path.Combine(SiteSourcePath, SiteTemplateFilename));
		}

		public static readonly string AssemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
		public static readonly string SiteSourcePath = Path.Combine(AssemblyPath, "Site");

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
			{
				copyResource(new Resource(site.ShortcutIcon_, string.Empty));
			}
		}

		void copyResource(Resource resource)
		{
			var targetPath = Path.Combine(_outputPath, resource.RelativeTargetPath);

			var dir = Path.GetDirectoryName(targetPath);
			Directory.CreateDirectory(dir);

			File.Copy(resource.SourcePath, targetPath, true);
		}

		void generatePage(SiteWriter site, Page page)
		{
			var context = new PageWriter(site, page);
			var html = _template.generateHTML(context);
			page.writePage(context, html);
		}
	}
}
	