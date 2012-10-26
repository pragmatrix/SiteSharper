using System.Web;

namespace SiteSharper.Reader
{
	static class ReadableURL
	{
		public static string read(string name)
		{
			var sanitized = name.ToLowerInvariant().Replace(" ", "-");
			return HttpUtility.UrlEncode(sanitized);
		}
	}
}
