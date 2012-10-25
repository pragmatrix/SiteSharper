﻿using System.IO;

namespace SiteSharper.Model
{
	public sealed class WordpressBlogPage : Page
	{
		readonly string _themeName;

		public WordpressBlogPage(string id, string name, string themeName)
			: base(id, name)
		{
			_themeName = themeName;
		}

		public override string Content
		{
			get
			{
				return string.Empty;
			}
		}

		const string TopId = "site-content-top";
		const string BottomId = "site-content-bottom";
		const string Head = "site-head";

		internal override void writePage(PageWriter writer, string html)
		{
			var siteWriter = writer.SiteWriter;

			var top = ExtractHTML.byId(html, TopId);
			var bottom = ExtractHTML.byId(html, BottomId);

			var contentPath = Path.Combine(Id, "wp-content");
			var themePath = Path.Combine(contentPath, "themes");
			var thisThemePath = Path.Combine(themePath, _themeName);

			siteWriter.writeHTML(Path.Combine(thisThemePath, TopId) + ".html", top);
			siteWriter.writeHTML(Path.Combine(thisThemePath, BottomId) + ".html", bottom);

			var header = ExtractHTML.contentOfElement(html, "head");
			siteWriter.writeHTML(Path.Combine(thisThemePath, Head + ".html" ), header);
		}
	}
}
