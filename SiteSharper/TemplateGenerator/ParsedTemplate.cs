using System.CodeDom;

namespace ProductSite.TemplateGenerator
{
	sealed class ParsedTemplate
	{
		public ParsedTemplate(string className, CodeCompileUnit compileUnit)
		{
			FullyQualifiedClassName = className;
			CompileUnit = compileUnit;
		}

		public readonly CodeCompileUnit CompileUnit;
		public readonly string FullyQualifiedClassName;
	}
}
