using System;
using System.Dynamic;
using System.Linq;

namespace ProductSite
{
	sealed class ModuleContextParameters : DynamicObject
	{
		readonly string[] _indexedArguments;

		public ModuleContextParameters(string[] indexedArguments)
		{
			_indexedArguments = indexedArguments;
		}

		public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
		{
			result = null;
			if (indexes.Length != 1)
				return false;

			if (indexes[0] == null || !indexes[0].GetType().Equals(typeof(int)))
				return false;

			var index = (int)indexes[0];
			if (index < 0 || index >= _indexedArguments.Length)
				throw new ArgumentOutOfRangeException("index");

			result = _indexedArguments[index];
			return true;
		}

		public static ModuleContextParameters fromReference(ModuleReference reference)
		{
			var argumentStrings = reference.Arguments.asStrings();

			return new ModuleContextParameters(argumentStrings.ToArray());
		}
	}
}
