using System.IO;
using System.Reflection;
using SiteSharper.Model;
using SiteSharper.Reader;
using SiteSharper.TemplateGenerator;
using SiteSharper.Writer;
using Toolbox;

namespace SiteSharper
{
	public sealed class SiteGenerator
	{
		readonly CompiledTemplate _pageTemplate;
		readonly CompiledTemplate _rssTemplate;
		readonly string _outputPath;

		public SiteGenerator(string outputPath)
		{
			_outputPath = outputPath;
			_pageTemplate = Template.compile<PageWriter>(Path.Combine(SiteSourcePath, SiteTemplateFilename));
			_rssTemplate = Template.compile<RSSFeedWriter>(Path.Combine(SiteSourcePath, RSSFeedTemplateFilename));
		}

		public static readonly string AssemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
		public static readonly string SiteSourcePath = Path.Combine(AssemblyPath, "Site");

		const string SiteTemplateFilename = "site.cshtml";
		const string RSSFeedTemplateFilename = "RSSFeed.cshtml";

		public void generate(Site site)
		{
			copyResources(site);
			generatePages(site);
		}

		void generatePages(Site site)
		{
			var writer = SiteWriter.create(site, _outputPath);

			foreach (var page in site.Pages)
				generatePage(writer, page);

			writer.Journals
				.forEach(j => generateJournalFeed(writer, j));

			writer.Journals
				.forEach(j => generateJournalPages(writer, j));
		}

		void generateJournalFeed(SiteWriter writer, JournalData journal)
		{
			var siteDomain = writer.Site.DomainName;
			var rssWriter = new RSSFeedWriter(journal, siteDomain);
			var res = _rssTemplate.generateXML(rssWriter);
			writer.writeFeed(rssWriter, res);
		}

		void generateJournalPages(SiteWriter writer, JournalData journal)
		{
			var indexPage = journal.createIndexPage();
			generatePage(writer, indexPage);

			journal.createPages()
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

			var sourceFile = resource.SourcePath;

			copyFile(sourceFile, targetPath);
		}

		void generatePage(SiteWriter siteWriter, IPage page)
		{
			var writer = new PageWriter(siteWriter, page);
			var html = _pageTemplate.generateHTML(writer);
			page.writePage(writer, html);
		}

		static void copyFile(string sourceFile, string targetPath)
		{
			var dir = Path.GetDirectoryName(targetPath);
			Directory.CreateDirectory(dir);
			File.Copy(sourceFile, targetPath, true);
		}
	}
}
	