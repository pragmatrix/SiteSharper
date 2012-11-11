using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Toolbox;

namespace SiteSharper.Model
{
	public sealed class Site
	{
		public Site(string name, string domainName, dynamic parameters)
		{
			Name = name;
			DomainName = domainName;
			Parameters = parameters;
			ModulesDirectories.Add(Path.Combine(SiteGenerator.AssemblyPath, StandardModulesDirectory));

			// these resources are required by our template for the time being.
			this.defaultSiteCSS();
			this.fancybox();
		}

		const string StandardModulesDirectory = "StandardModules";

		public readonly string Name;
		public readonly string DomainName;
		public string Slogan = string.Empty;
		public string Description = string.Empty;
		public Page HomePage_;
		public string Logo_;
		public Menu Menu = new Menu();
		public Menu FooterMenu = new Menu();
		public readonly List<Resource> Resources = new List<Resource>();
		public readonly List<Feed> Feeds = new List<Feed>();
		public readonly List<CSSRef> CSSReferences = new List<CSSRef>();
		public readonly List<string> ModulesDirectories = new List<string>();
		public dynamic Parameters;
		public readonly List<string> TrackingCodeFiles = new List<string>();
		public string ShortcutIcon_;
		public readonly List<Journal> Journals = new List<Journal>();
		public string PageFileExtension = string.Empty;
		public List<Two<string>>  Mirrors = new List<Two<string>>();

		public Site logo(string logo)
		{
			Logo_ = logo;
			return this;
		}

		public Site slogan(string slogan)
		{
			Slogan = slogan;
			return this;
		}

		public Site description(string description)
		{
			Description = description;
			return this;
		}

		public Site home(Page page)
		{
			HomePage_ = page;
			return this;
		}

		public Site menu(Menu menu)
		{
			Menu = menu;
			return this;
		}

		public Site footerMenu(Menu menu)
		{
			FooterMenu = menu;
			return this;
		}

		public Site resource(string sourcePath, string relativeTargetPath = "")
		{
			if (relativeTargetPath == "")
				relativeTargetPath = Path.GetFileName(sourcePath);

			Resources.Add(new Resource(sourcePath, relativeTargetPath));
			return this;
		}

		public Site feed(Feed feed)
		{
			Feeds.Add(feed);
			return this;
		}

		public Site css(string file)
		{
			resource(file);
			cssRef("/" + file);
			return this;
		}

		public Site cssRef(string absoluteURL)
		{
			CSSReferences.Add(new CSSRef(absoluteURL));
			return this;
		}

		public Site journal(Journal journal)
		{
			Journals.Add(journal);
			var journalFeed = new Feed(journal.Title, "http://" + DomainName + "/" + journal.FeedSitePath);
			feed(journalFeed);

			this.resources(journal.Id, journal.Id);
			return this;
		}

		public Site mirror(string from, string to)
		{
			Mirrors.Add(Two.make(from, to));
			return this;
		}

		#region File System Binding

		public Site useModulesInDirectory(string directory)
		{
			ModulesDirectories.Add(directory);
			return this;
		}

		public Site trackingCodeFile(string file)
		{
			TrackingCodeFiles.Add(file);
			return this;
		}

		public Site shortcutIcon(string file)
		{
			ShortcutIcon_ = file;
			return this;
		}

		public Site pageFileExtension(string ext)
		{
			PageFileExtension = ext;
			return this;
		}

		#endregion

		#region Queries

		public IEnumerable<Page> Pages
		{
			get
			{
				var pages = Menu.Pages.Concat(FooterMenu.Pages);

				if (HomePage_ != null)
					pages = pages.append(HomePage_);

				return pages.Distinct();
			}
		}

		public string urlOf(Page page)
		{
			if (page.URL_ != null)
				return page.URL_;

			if (page == HomePage_)
				return "/";

			return page.Id;
		}

		#endregion

		#region Helpers

		internal string sitePathOf(Page page)
		{
			if (page.URL_ != null)
				throw new Exception("This page has no file associated");

			if (page == HomePage_)
				return "index.html";

			return page.Id + PageFileExtension;
		}

		public string renderFeeds()
		{
			var sb = new StringBuilder();

			foreach (var feed in Feeds)
			{
				sb.Append(feed.render());
			}

			return sb.ToString();
		}

		public string renderCSSReferences()
		{
			var sb = new StringBuilder();

			foreach (var cssRef in CSSReferences)
			{
				sb.Append(cssRef.render());
			}

			return sb.ToString();
		}

		#endregion
	}
}
