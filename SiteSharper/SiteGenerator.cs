using System;
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
			copyMirrors(site);
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

			var sourceFile = resource.SourcePath;

			copyFile(sourceFile, targetPath);
		}

		void generatePage(SiteWriter siteWriter, Page page)
		{
			var writer = new PageWriter(siteWriter, page);
			var html = _pageTemplate.generateHTML(writer);
			page.writePage(writer, html);
		}

		void copyMirrors(Site site)
		{
			site.Mirrors.forEach(t => copyMirror(t.First, t.Second));
		}

		void copyMirror(string from, string to)
		{
			var sourcePath = Path.Combine(_outputPath, from);
			if (!File.Exists(sourcePath))
				throw new Exception("mirror source file {0} does not exist".format(from));

			var targetPath = Path.Combine(_outputPath, to);
			copyFile(sourcePath, targetPath);
		}

		static void copyFile(string sourceFile, string targetPath)
		{
			var dir = Path.GetDirectoryName(targetPath);
			Directory.CreateDirectory(dir);
			File.Copy(sourceFile, targetPath, true);
		}
	}
}
	