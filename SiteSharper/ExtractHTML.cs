using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using Toolbox;

namespace SiteSharper
{
	static class ExtractHTML
	{
		public static string byId(string html, string id)
		{
			var xml = loadWithoutDTD(html);

			var element = elementById(xml, id);
			return element.OuterXml;
		}

		public static string contentOfElement(string html, string element)
		{
			var xml = loadWithoutDTD(html);

			var elements = childElementsOf(xml, element);
			var xmls = elements.Select(e => e.OuterXml);
			return string.Join("\n", xmls);
		}

		// http://stackoverflow.com/questions/215854/prevent-dtd-download-when-parsing-xml

		public static XmlDocument loadWithoutDTD(string html)
		{
			var settings = new XmlReaderSettings
			{
				XmlResolver = null, 
				DtdProcessing = DtdProcessing.Ignore
			};
			var doc = new XmlDocument
			{
				PreserveWhitespace = true
			};

			using (var sr = new StringReader(html))
			using (var reader = XmlReader.Create(sr, settings))
				doc.Load(reader);

			return doc;
		}

		/*
			This expects a HTML document structured without a DOCTYPE beginning and ending with 
			the html element <html>CONTENT</html>
		*/

		public static XmlDocument loadHTML(string html)
		{
			var settings = new XmlReaderSettings
			{
				XmlResolver = null,
				DtdProcessing = DtdProcessing.Parse
			};
			var doc = new XmlDocument
			{
				PreserveWhitespace = true
			};

			using (var sr = new StringReader(HTMLDocType + html))
			using (var reader = XmlReader.Create(sr, settings))
				doc.Load(reader);

			removeDocumentType(doc);

			return doc;
		}


		// todo: add missing html entities.

		const string HTMLDocType = "<!DOCTYPE html[" +
			"<!ENTITY nbsp \"&#160;\">" +
			"<!ENTITY hellip \"&#133;\">" +
			"<!ENTITY ldquo \"&#8220;\">" +
			"<!ENTITY rdquo \"&#8221;\">" +
			"<!ENTITY ndash \"&#8211;\">" +
			"]>\n";

		// GetElementById works only if the DTD specified it.

		public static XmlElement elementById(XmlDocument document, string id)
		{
			var list = document.SelectNodes("//*[@id='{0}']".format(id));

			if (list.Count != 1)
				throw new Exception("Failed to find element {0} in HTML document".format(id));

			return (XmlElement)list[0];
		}

		public static IEnumerable<XmlElement> childElementsOf(XmlDocument document, string element)
		{
			var list = document.SelectNodes("//{0}/*".format(element));
			return list.Cast<XmlElement>();
		}

		static void removeDocumentType(XmlDocument doc)
		{
			foreach (var n in doc.ChildNodes)
			{
				var docTypeNode = n as XmlDocumentType;
				if (docTypeNode != null)
				{
					doc.RemoveChild(docTypeNode);
					break;
				}
			}
		}
	}
}
