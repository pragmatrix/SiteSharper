using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using SiteSharper.Model;
using SiteSharper.Reader;

namespace SiteSharper.Writer
{
	public sealed class SiteWriter
	{
		readonly string _outputPath;
		readonly ModuleProcessor _moduleProcessor = new ModuleProcessor();

		public Site Site { get; private set; }
		public string TrackingCode { get; private set; }
		public string ShortcutIconFilename_ { get; private set; }
		public readonly JournalData[] Journals;

		SiteWriter(Site site, string outputPath, string trackingCode, string shortcutIconFilename_)
		{
			_outputPath = outputPath;
			Site = site;
			TrackingCode = trackingCode;
			ShortcutIconFilename_ = shortcutIconFilename_;

			Journals = site.Journals
				.Select(JournalData.read)
				.ToArray();
		}

		public string postProcess(PageWriter pageWriter, string html)
		{
			return _moduleProcessor.postProcess(pageWriter, html);
		}

		public static SiteWriter create(Site site, string outputPath)
		{
			var trackingCode = readTrackingCode(site.TrackingCodeFiles);
			var shortcutIconFilename_ = tryGetShortcutIconFilename(site.ShortcutIcon_);
	
			return new SiteWriter(site, outputPath, trackingCode, shortcutIconFilename_);
		}

		static string readTrackingCode(IEnumerable<string> files)
		{
			var tc = new StringBuilder();

			foreach (var f in files)
			{
				var text = File.ReadAllText(f, Encoding.UTF8);
				tc.Append(text);
			}

			return tc.ToString();
		}

		static string tryGetShortcutIconFilename(string fn_)
		{
			if (fn_ == null)
				return null;

			return Path.GetFileName(fn_);
		}

		public JournalData journalFor(string id)
		{
			return Journals.Single(j => j.Journal.Id == id);
		}

		public void writePage(Page page, string html)
		{
			writeText(Site.sitePathOf(page), html);
		}

		public void writeFeed(RSSFeedWriter writer, string feed)
		{
			writeText(writer.SitePath, feed);
		}

		public void writeText(string relativePath, string html)
		{
			var outputPath = Path.Combine(_outputPath, relativePath);
			var outputDir = Path.GetDirectoryName(outputPath);
			Directory.CreateDirectory(outputDir);
			File.WriteAllText(outputPath, html);
		}
	}
}
