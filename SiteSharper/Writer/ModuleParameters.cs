using System;
using System.Collections.Specialized;
using System.Dynamic;
using System.Linq;

namespace SiteSharper.Writer
{
	sealed class ModuleParameters : DynamicObject
	{
		readonly string[] _indexedArguments;
		readonly NameValueCollection _namedArguments;

		public ModuleParameters(string[] indexedArguments, NameValueCollection namedArguments)
		{
			_indexedArguments = indexedArguments;
			_namedArguments = namedArguments;
		}


		public override bool TryGetMember(GetMemberBinder binder, out object result)
		{
			result = _namedArguments.GetValues(binder.Name).Single();
			return true;
		}

		public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
		{
			result = null;
			if (indexes.Length != 1)
				return false;

			if (indexes[0] == null || !(indexes[0] is int))
				return false;

			var index = (int)indexes[0];
			if (index < 0 || index >= _indexedArguments.Length)
				throw new ArgumentOutOfRangeException("index");

			result = _indexedArguments[index];
			return true;
		}

		public static ModuleParameters fromReference(ModuleReference reference)
		{
			var argumentStrings = reference.Arguments.asStrings();

			return new ModuleParameters(argumentStrings.ToArray(), reference.NamedValues);
		}
	}
}
