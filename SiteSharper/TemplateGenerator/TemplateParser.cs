using System;
using System.IO;
using System.Web.Razor;
using System.Web.Razor.Parser;

namespace SiteSharper.TemplateGenerator
{
	static class TemplateParser
	{
		const string GeneratedTemplateNamespace = "GeneratedTemplates";

		public static ParsedTemplate parse(string sourceFilename, string cshtmlContent, string effectiveTemplateClassName, Type modelType)
		{
			var csCodeLanguage = new CSharpRazorCodeLanguage();
			var templateHost = new RazorEngineHost(csCodeLanguage, () => new HtmlMarkupParser());

			var concreteBaseClassType = getBaseClassTypeFromModel(modelType);
			templateHost.DefaultBaseClass = concreteBaseClassType.FullName;

			var templateEngine = new RazorTemplateEngine(templateHost);

			var trimmedcshtmlContent = HeaderLines.trim(cshtmlContent);

			GeneratorResults res;
			using (var input = new StringReader(trimmedcshtmlContent))
			{
				res = templateEngine.GenerateCode(input, effectiveTemplateClassName, GeneratedTemplateNamespace, sourceFilename);
			}

			if (!res.Success)
				throw new Exception("Failed to generate code");

			var compileUnit = res.GeneratedCode;
			var fullyQualifiedClassName = GeneratedTemplateNamespace + "." + effectiveTemplateClassName;
	
			return new ParsedTemplate(fullyQualifiedClassName, compileUnit);
		}

		static Type getBaseClassTypeFromModel(Type modelType)
		{
			var baseClassType = typeof(RazorHtmlTemplate<>);
			var concreteBaseClassType = baseClassType.MakeGenericType(new[] { modelType });
			return concreteBaseClassType;
		}
	}
}
