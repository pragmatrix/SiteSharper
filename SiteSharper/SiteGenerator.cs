using System.IO;
using System.Reflection;
using SiteSharper.Model;
using SiteSharper.Reader;
using SiteSharper.TemplateGenerator;
using Toolbox;

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
			var writer = SiteWriter.create(site, _outputPath);
			
			foreach (var page in site.Pages)
			{
				if (page.URL_ != null)
					continue;

				generatePage(writer, page);
			}

			writer.Journals
				.forEach(j => generateJournalPages(writer, j));
		}

		void generateJournalPages(SiteWriter writer, JournalData journal)
		{
			journal.createEntryPages()
				.forEach(p => generatePage(writer, p));
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

		void generatePage(SiteWriter siteWriter, Page page)
		{
			var writer = new PageWriter(siteWriter, page);
			var html = _template.generateHTML(writer);
			page.writePage(writer, html);
		}
	}
}
	