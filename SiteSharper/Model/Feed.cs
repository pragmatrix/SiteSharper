using System.Web;
using Toolbox;

namespace ProductSite.Model
{
	public sealed class Feed
	{
		public readonly string _name;
		public readonly string _absoluteURL;

		public Feed(string name, string absoluteURL)
		{
			_name = name;
			_absoluteURL = absoluteURL;
		}

		public string render()
		{
			return "<link rel='alternate' type='application/rss+xml' title='{0}' href='{1}'/>\n".
				format(HttpUtility.HtmlAttributeEncode(_name), 
				HttpUtility.HtmlAttributeEncode(_absoluteURL));
		}
	}
}
