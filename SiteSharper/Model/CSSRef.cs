using System.Web;
using Toolbox;

namespace SiteSharper.Model
{
	public sealed class CSSRef
	{
		public readonly string URL;

		public CSSRef(string url)
		{
			URL = url;
		}

		public string render()
		{
			return "<link rel='stylesheet' type='text/css' href='{0}'/>\n".
				format(HttpUtility.HtmlAttributeEncode(URL));
		}
	}
}
