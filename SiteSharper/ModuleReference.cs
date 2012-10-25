using System;
using System.Collections.Specialized;
using System.Web;
using System.Xml;

namespace SiteSharper
{
	sealed class ModuleReference
	{
		public readonly Uri URI;
		public readonly XmlElement Element;
		public readonly ModuleReferenceArguments Arguments;
		public readonly NameValueCollection NamedValues;

		ModuleReference(Uri uri, XmlElement element, ModuleReferenceArguments arguments)
		{
			URI = uri;
			Element = element;
			Arguments = arguments;
			NamedValues = HttpUtility.ParseQueryString(uri.Query);
		}

		public static ModuleReference extractByLink(Uri uri, XmlElement element)
		{
			element = findReplaceableElement(element);
			var arguments = ModuleReferenceArguments.scan(element);
			return new ModuleReference(uri, element, arguments);

		}

		static XmlElement findReplaceableElement(XmlElement linkElement)
		{
			// if the link node is single and enclosed in a paragraph, then replace the paragraph.
			var parent = (XmlElement) linkElement.ParentNode;
			if (parent.Name == "p" && parent.ChildNodes.Count == 1)
				return parent;
			return linkElement;
		}

		public void expand(XmlNodeList nodeList)
		{
			var element = Element;
			var doc = element.OwnerDocument;
			var parent = element.ParentNode;
			foreach (XmlNode node in nodeList)
			{
				var imported = doc.ImportNode(node, true);
				parent.InsertBefore(imported, element);
			}

			parent.RemoveChild(element);
			Arguments.removeArgumentsFromReferringDocument();
		}
	}
}