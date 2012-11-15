using System.Collections.Generic;
using System.Linq;
using System.Web;
using Toolbox;
using SiteSharper.Reader;

namespace SiteSharper
{
	sealed class ModuleCall
	{
		readonly string _moduleName;
		readonly List<Two<string>> _arguments = new List<Two<string>>();

		public ModuleCall(string moduleName)
		{
			_moduleName = moduleName;
		}

		public ModuleCall argument(string name, string value)
		{
			_arguments.Add(Two.make(name, value));
			return this;
		}

		public string toHTML()
		{
			string arguments = _arguments
					.Select(a => a.First + "=" + HttpUtility.UrlEncode(a.Second))
					.Aggregate("", (a, b) => a + "&" + b);

			var markdown = "[](module:{0}{1}{2})".format(
				_moduleName, 
				arguments != "" ? "?" : "", arguments);

			return MarkdownReader.fromString(markdown);
		}
	}
}
