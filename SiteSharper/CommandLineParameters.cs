using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Toolbox;

namespace SiteSharper
{
	public sealed class CommandLineParameters : DynamicObject
	{
		readonly Dictionary<string, string> _parameters;

		CommandLineParameters(Dictionary<string, string> parameters)
		{
			_parameters = parameters;
		}

		public bool has(string key)
		{
			return _parameters.ContainsKey(key);
		}

		public void add(string key, string value)
		{
			_parameters.Add(key, value);
		}

		public override bool TryGetMember(GetMemberBinder binder, out object result)
		{
			string str;
			var gotIt = _parameters.TryGetValue(binder.Name, out str);
			result = str;
			return gotIt;
		}

		public static CommandLineParameters create(IEnumerable<string> commandLineArguments)
		{
			return new CommandLineParameters(parseCommandLineParameters(commandLineArguments));
		}


		static Dictionary<string, string> parseCommandLineParameters(IEnumerable<string> parameters)
		{
			return parameters
				.Select(tryParseCommandLineParameter)
				.Where(p => p != null).ToDictionary(p => p.Value.First, p => p.Value.Second);
		}

		static Two<string>? tryParseCommandLineParameter(string parameter)
		{
			if (!parameter.StartsWith("-p:"))
				return null;

			var str = parameter.Substring(3);
			var equalIndex = str.IndexOf('=');
			if (equalIndex == -1)
				return null;

			var left = str.Substring(0, equalIndex);
			if (left.Length == 0)
				return null;

			var right = str.Substring(equalIndex + 1);
			return Two.make(left, right);
		}
	}
}
