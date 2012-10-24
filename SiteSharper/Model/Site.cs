using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Toolbox;

namespace SiteSharper.Model
{
	public sealed class Site
	{
		public Site(string name, dynamic parameters)
		{
			Name = name;
			Parameters = parameters;
		}

		public readonly string Name;
		public string Slogan = string.Empty;
		public string Description = string.Empty;
		public Page HomePage_;
		public string Logo_;
		public Menu Menu = new Menu();
		public Menu FooterMenu = new Menu();
		public readonly List<string> Resources = new List<string>();
		public readonly List<Feed> Feeds = new List<Feed>();
		public string ModulesDirectory_;
		public dynamic Parameters;
		public readonly List<string> TrackingCodeFiles = new List<string>();
		public string ShortcutIcon_;

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

		public Site resource(string path)
		{
			Resources.Add(path);
			return this;
		}

		public Site feed(Feed feed)
		{
			Feeds.Add(feed);
			return this;
		}

		public Site useModulesInDirectory(string directory)
		{
			ModulesDirectory_ = directory;
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

		internal string filenameOf(Page page)
		{
			if (page.URL_ != null)
				throw new Exception("This page has no file associated");

			if (page == HomePage_)
				return "index.html";

			return page.Id;
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

		#endregion
	}
}
