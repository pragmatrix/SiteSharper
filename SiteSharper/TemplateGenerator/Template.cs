using System;
using System.IO;
using Toolbox;

namespace ProductSite.TemplateGenerator
{
	static class Template
	{
		const string TemplateClassName = "{0}Template";

		public static CompiledTemplate compile<ModelTypeT>(string templateFilename)
		{
			var templateName = Path.GetFileNameWithoutExtension(templateFilename);
			var templateContent = File.ReadAllText(templateFilename);

			return compile(templateFilename, templateName, templateContent, typeof(ModelTypeT));
		}

		public static string generateHTML(string templateFilename, object model)
		{
			var templateName = Path.GetFileNameWithoutExtension(templateFilename);
			var templateContent = File.ReadAllText(templateFilename);

			var html = generateHTML(templateFilename, templateName, templateContent, model);

			return html;
		}

		static string generateHTML(string sourceFilename, string templateName, string templateContent, object model)
		{
			var compiled = compile(sourceFilename, templateName, templateContent, model.GetType());

			var html = compiled.generateHTML(model);
			return html;
		}

		static CompiledTemplate compile(string sourceFilename, string templateName, string templateContent, Type modelType)
		{
			var templateClassName = TemplateClassName.format(templateName);

			var parsed = TemplateParser.parse(sourceFilename, templateContent, templateClassName, modelType);
			var compiled = parsed.compile();
			return compiled;
		}
	}
}
