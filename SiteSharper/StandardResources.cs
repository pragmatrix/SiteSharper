using System.IO;
using SiteSharper.Model;

namespace SiteSharper
{
	public static class StandardResources
	{
		public static Site defaultSiteCSS(this Site site)
		{
			return site.resource(Path.Combine(SiteGenerator.SiteSourcePath, "site.css"));
		}

		public static Site fancybox(this Site site)
		{
			return site.resources(Path.Combine(SiteGenerator.SiteSourcePath, "fancybox"), "fancybox");
		}

		public static Site fontAwesome(this Site site)
		{
			site.css(Path.Combine(SiteGenerator.SiteSourcePath, "font-awesome/css/font-awesome.css"), "font-awesome/css/font-awesome.css");

			site.resources(Path.Combine(SiteGenerator.SiteSourcePath, "font-awesome/font"), "font-awesome/font");
			return site.resources(Path.Combine(SiteGenerator.SiteSourcePath, "font-awesome/css"), "font-awesome/css");
		}

		public static Site uservoice(this Site site, string id)
		{
			var call = new ModuleCall("UserVoiceHead")
				.argument("id", id);

			return site.header(call.toHTML());
		}
	}
}
