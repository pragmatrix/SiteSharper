using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace ProductSite
{
	public sealed class ModuleReferenceArguments
	{
		readonly IEnumerable<XmlNode> _nodes;
		readonly IEnumerable<XmlDocumentFragment> _arguments;

		ModuleReferenceArguments(IEnumerable<XmlNode> nodes, IEnumerable<XmlDocumentFragment> arguments)
		{
			_nodes = nodes;
			_arguments = arguments;
		}

		public IEnumerable<string> asStrings()
		{
			return _arguments.Select(arg => arg.OuterXml);
		}

		public void removeArgumentsFromReferringDocument()
		{
			foreach (var n in _nodes)
			{
				var parent = (XmlElement)n.ParentNode;
				parent.RemoveChild(n);
			}
		}

		public static ModuleReferenceArguments scan(XmlElement placeholder)
		{
			var consumed = new List<XmlNode>();
			var arguments = new List<XmlDocumentFragment>();

			var content = tryFindNextNonWhitespaceSibling(placeholder);
			if (content == null)
				return NoArguments;
			if (!isSignatureNode(content, "["))
				return NoArguments;

			consumed.Add(content);
			var currentArg = createFragment();

			for (; ; )
			{
				content = content.NextSibling;
				if (content == null)
					return NoArguments;

				consumed.Add(content);

				if (isSignatureNode(content, "]"))
				{
					arguments.Add(currentArg);
					break;
				}

				if (isSignatureNode(content, ","))
				{
					arguments.Add(currentArg);
					currentArg = createFragment();
					continue;
				}

				currentArg.AppendChild(currentArg.OwnerDocument.ImportNode(content, true));
			}

			arguments.ForEach(trimWhitespace);

			return new ModuleReferenceArguments(consumed, arguments);
		}

		static XmlDocumentFragment createFragment()
		{
			var doc = new XmlDocument();
			return doc.CreateDocumentFragment();
		}

		static void trimWhitespace(XmlDocumentFragment fragment)
		{
			while (fragment.FirstChild != null && fragment.FirstChild.NodeType == XmlNodeType.Whitespace)
				fragment.RemoveChild(fragment.FirstChild);

			while (fragment.LastChild != null && fragment.LastChild.NodeType == XmlNodeType.Whitespace)
				fragment.RemoveChild(fragment.LastChild);
		}

		static bool isSignatureNode(XmlNode node, string signature)
		{
			return node.NodeType == XmlNodeType.Element && node.Name == "p" && node.InnerText == signature;
		}

		static XmlNode tryFindNextNonWhitespaceSibling(XmlNode node)
		{
			for(;;)
			{
				node = node.NextSibling;
				if (node == null || node.NodeType != XmlNodeType.Whitespace)
					return node;
			}
		}

		static readonly ModuleReferenceArguments NoArguments = new ModuleReferenceArguments(
			Enumerable.Empty<XmlElement>(), 
			Enumerable.Empty<XmlDocumentFragment>());
	}
}