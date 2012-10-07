using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using ProductSite.Model;
using ProductSite.TemplateGenerator;

namespace ProductSite
{
	sealed class ModuleProcessor
	{
		readonly Dictionary<string, CompiledTemplate> _moduleTemplates = new Dictionary<string, CompiledTemplate>();

		public string postProcess(PageContext context, string xml)
		{
			bool anyModulesReplaced;
			var document = ExtractHTML.loadWithoutDTD(xml);
			do
			{
				anyModulesReplaced = false;
				var references = findModuleReferences(document).ToArray();

				foreach (var reference in references)
				{
					var moduleName = reference.URI.LocalPath;
					var template = resolveModuleTemplate(context.Site, moduleName);

					var parameters = ModuleContextParameters.fromReference(reference);
					var moduleContext = new ModuleContext(context, parameters);
					var moduleHTML = "<root>" + template.generateHTML(moduleContext) + "</root>";
					var moduleDocument = ExtractHTML.loadWithoutDTD(moduleHTML);
					reference.expand(moduleDocument.DocumentElement.ChildNodes);

					anyModulesReplaced = true;
				}

			} while (anyModulesReplaced);

			return XHTMLWriter.writeStrict(document);
		}

		IEnumerable<ModuleReference> findModuleReferences(XmlDocument document)
		{
			foreach (XmlElement element in document.SelectNodes("//a"))
			{
				var href = element.Attributes["href"];
				if (href == null)
					continue;

				Uri uri;
				if (!Uri.TryCreate(href.Value, UriKind.Absolute, out uri))
					continue;

				if (uri.Scheme != "module")
					continue;

				yield return ModuleReference.extractByLink(uri, element);
			}
		}


		CompiledTemplate resolveModuleTemplate(Site site, string name)
		{
			CompiledTemplate template;
			if (!_moduleTemplates.TryGetValue(name, out template))
			{
				template = loadModuleTemplate(site, name);
				_moduleTemplates.Add(name, template);
			}

			return template;
		}

		CompiledTemplate loadModuleTemplate(Site site, string name)
		{
			if (site.ModulesDirectory_ == null)
				throw new Exception("Failed to load module template {0}: no modules directory set in site");

			var dir = site.ModulesDirectory_;
			var fn = Path.Combine(dir, name + ".cshtml");
			return Template.compile<ModuleContext>(fn);
		}
	}
}
