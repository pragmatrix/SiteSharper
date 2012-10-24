using System.Web;
using Toolbox;

namespace SiteSharper.Model
{
	public sealed class Feed
	{
		public readonly string Name;
		public readonly string AbsoluteURL;

		public Feed(string name, string absoluteURL)
		{
			Name = name;
			AbsoluteURL = absoluteURL;
		}

		public string render()
		{
			return "<link rel='alternate' type='application/rss+xml' title='{0}' href='{1}'/>\n".
				format(HttpUtility.HtmlAttributeEncode(Name), 
				HttpUtility.HtmlAttributeEncode(AbsoluteURL));
		}
	}
}
