using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml;

namespace ProductSite
{
	static class XHTMLWriter
	{
		public static string writeStrict(XmlDocument original)
		{
			Debug.Assert(original.DocumentType == null);
			const string DocType = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Strict//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd\">";
			var document = (XmlDocument)original.Clone();
			addHTMLStringNamespaceToRootElement(document);

			using (var stringWriter = new StringWriter())
			{
				using (var textWriter = new XHTMLTextWriter(stringWriter))
				{
					document.WriteTo(textWriter);
				}

				return DocType + stringWriter;
			}
		}

		static void addHTMLStringNamespaceToRootElement(XmlDocument document)
		{
			var xmlns = "http://www.w3.org/1999/xhtml";
			var rootElement = document.DocumentElement;
			Debug.Assert(rootElement.Name == "html");
			var attr = document.CreateAttribute("xmlns");
			attr.Value = xmlns;
			rootElement.Attributes.Append(attr);
		}

		sealed class XHTMLTextWriter : XmlTextWriter
		{
			readonly Stack<string> _elements = new Stack<string>();

			public XHTMLTextWriter(TextWriter textWriter)
				: base(textWriter)
			{
			}


			public override void WriteStartElement(string prefix, string localName, string ns)
			{
				base.WriteStartElement(prefix, localName, ns);
				_elements.Push(localName);
			}

			// for some reason <script> and similar elements can not be closed immediately, 
			// so we always write a full end element except for <br>.

			public override void WriteEndElement()
			{
				var current = _elements.Pop();
				if (Array.IndexOf(XHTMLShortableElements, current.ToLowerInvariant()) != -1)
					base.WriteEndElement();
				else
					WriteFullEndElement();
			}

			static readonly string[] XHTMLShortableElements = new[] { "br", "link", "meta"};
		}
	}
}
