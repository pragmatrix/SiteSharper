using System.Linq;

namespace SiteSharper.Reader
{
	static class ReadableURL
	{
		public static string read(string name)
		{
			return new string(name
				.ToLowerInvariant()
				.Replace(" ", "-")
				.Where(c => !isBadCharacter(c))
				.ToArray());
		}

		static bool isBadCharacter(char c)
		{
			return BadCharacters.IndexOf(c) != -1;
		}
		// http://www.blooberry.com/indexdot/html/topics/urlencoding.htm
		// And we also add "().", because Drupal does.
		const string BadCharacters = "$&+,/:;=?@\"<>#%{}|\\^~[]`().";
	}
}
