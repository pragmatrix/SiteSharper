using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using SiteSharper.Model;
using SiteSharper.Readers;

namespace SiteSharper
{
	public sealed class SiteContext
	{
		readonly ModuleProcessor _moduleProcessor = new ModuleProcessor();

		public Site Site { get; private set; }
		public string TrackingCode { get; private set; }
		public string ShortcutIconFilename_ { get; private set; }
		JournalData[] _journals;

		SiteContext(Site site, string trackingCode, string shortcutIconFilename_)
		{
			Site = site;
			TrackingCode = trackingCode;
			ShortcutIconFilename_ = shortcutIconFilename_;

			_journals = site.Journals
				.Select(JournalData.read)
				.ToArray();
		}

		public string postProcess(PageContext pageContext, string html)
		{
			return _moduleProcessor.postProcess(pageContext, html);
		}

		public static SiteContext create(Site site)
		{
			var trackingCode = readTrackingCode(site.TrackingCodeFiles);
			var shortcutIconFilename_ = tryGetShortcutIconFilename(site.ShortcutIcon_);
	
			return new SiteContext(site, trackingCode, shortcutIconFilename_);
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
			return _journals.Single(j => j.Journal.Id == id);
		}
	}
}
