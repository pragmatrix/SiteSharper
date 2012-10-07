using System.Reflection;

namespace SiteSharper.TemplateGenerator
{
	sealed class CompiledTemplate
	{
		public CompiledTemplate(string className, Assembly assembly)
		{
			Assembly = assembly;
			FullyQualifiedClassName = className;
		}

		public readonly Assembly Assembly;
		public readonly string FullyQualifiedClassName;
	}
}
