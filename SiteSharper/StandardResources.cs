using System.IO;
using SiteSharper.Model;

namespace SiteSharper
{
	public static class StandardResources
	{
		public static Site fancybox(this Site site)
		{
			return site.resources(Path.Combine(SiteGenerator.SiteSourcePath, "fancybox"), "fancybox");
		}

		public static Site defaultSiteCSS(this Site site)
		{
			return site.resource(Path.Combine(SiteGenerator.SiteSourcePath, "site.css"));
		}
		 
		public static Site uservoice(this Site site, string id)
		{
			var call = new ModuleCall("UserVoiceHead")
				.argument("id", id);

			return site.header(call.toHTML());
		}
	}
}
