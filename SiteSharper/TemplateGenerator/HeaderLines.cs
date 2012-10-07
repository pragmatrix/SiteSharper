using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SiteSharper.TemplateGenerator
{
	static class HeaderLines
	{
		public static string trim(string input)
		{
			var result = new List<string>();
			var lines = splitLines(input);

			using (var enumerator = lines.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					var current = enumerator.Current;
					if (matchHeaderLine(current))
						continue;
					
					result.Add(current);
					while (enumerator.MoveNext())
						result.Add(enumerator.Current);

					return result.Aggregate((a, b) => a + "\n" + b);
				}
			}

			return String.Empty;
		}

		static IEnumerable<string> splitLines(string input)
		{
			using (var lines = new StringReader(input))
			{
				while (lines.Peek() != -1)
				{
					yield return lines.ReadLine();
				}
			}
		}

		static bool matchHeaderLine(string line)
		{
			var trimmed = line.Trim();
			if (trimmed.Length == 0)
				return true;
			if (trimmed.Length > 1 && trimmed[0] == '@')
			{
				var nextSpace = trimmed.IndexOfAny(SpaceCharacters);
				if (nextSpace == -1)
					nextSpace = trimmed.Length;

				var keyword = trimmed.Substring(1, nextSpace - 1);
				if (Array.IndexOf(HeaderKeywords, keyword) != -1)
					return true;
			}

			return false;
		}

		static readonly char[] SpaceCharacters = new char[]
		{
			' ', '\t'
		};

		static readonly string[] HeaderKeywords = new string[]
		{
			"model"
		};
	}
}
