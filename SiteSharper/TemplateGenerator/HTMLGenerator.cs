using System;
using System.Reflection;

namespace SiteSharper.TemplateGenerator
{
	static class HTMLGenerator
	{
		public static string generateHTML(this CompiledTemplate compiledTemplate, object model)
		{
			return generateHTML(compiledTemplate.Assembly, compiledTemplate.FullyQualifiedClassName, model);
		}

		static string generateHTML(Assembly assembly, string fullyQualitiedTemplateClassName, object model)
		{
			var generatedType = assembly.GetType(fullyQualitiedTemplateClassName);
			var instance = (RazorHtmlTemplate) Activator.CreateInstance(generatedType);
			var html = instance.generate(model);
			return html;
		}
	}
}
