using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using SiteSharper.Model;
using Toolbox;

namespace SiteSharper.Reader
{
	public sealed class JournalData
	{
		public Journal Journal;
		public JournalEntry[] Entries;

		public static JournalData read(Journal journal)
		{
			var entries = Directory.EnumerateFiles(journal.Path)
				.Where(isJournalFileType)
				.Select(f => loadJournalEntry(journal, f))
				.OrderByDescending(entry => entry.Filename.ToString())
				.ToArray();

			return new JournalData {Journal = journal, Entries = entries};
		}

		static JournalEntry loadJournalEntry(Journal journal, string filePath)
		{
			var filename = JournalEntryFilename.fromFilename(Path.GetFileName(filePath));
			var content = readJournalContent(filePath);
			var entryId = journal.Id + "/" + ReadableURL.read(filename.ToString());

			var header = "[](module:BlogEntryHeader?entry={0}&name={1}&date={2})".format(
				HttpUtility.UrlEncode(entryId), 
				HttpUtility.UrlEncode(filename.NamePart),
				HttpUtility.UrlEncode(DateReader.printDateTimeCode(filename.DateTimeCode)));

			var headerHTML = MarkdownReader.fromString(header);

			var date = DateReader.fromDateTimeCode(filename.DateTimeCode);

			return new JournalEntry
			{
				Id = entryId, 
				Filename = filename, 
				Content = headerHTML + content,
				Date = date
			};
		}

		public Page createIndexPage()
		{
			var call = new ModuleCall("JournalIndex")
				.argument("journal", Journal.Id);

			var page =  new Page(Journal.Id + "/index", Journal.Title)
			       {
				       Content = call.toHTML()
			       };

			if (Journal.Search_ != null)
				page.Header = Journal.Search_.Header;

			return page;
		}

		public IEnumerable<Page> createPages()
		{
			return Entries.Select(createPage);
		}

		Page createPage(JournalEntry entry)
		{
			var filename = entry.Filename;
			var fileId = ReadableURL.read(filename.ToString());
			var pageId = Journal.Id + "/" + fileId;

			string footer = "";
			if (Journal.Comments_ != null)
				footer = Journal.Comments_.createScriptForCommentArea("/" + pageId);

			return new Page(pageId, filename.NamePart)
			       {
				       Content = entry.Content + footer
			       };
		}


		public IEnumerable<JournalEntry> FeedEntries
		{
			get
			{
				var feedSettings = Journal.FeedSettings;
				var index = 0;
				var cutOffDate = DateTime.Now + feedSettings.MinimumTimeSpanToCover;

				foreach (var entry in Entries)
				{
					if (index < feedSettings.MinimumNumberOfItems || entry.Date < cutOffDate)
						yield return entry;
					else
						break;

					++index;
				}
			}
		}


		static string readJournalContent(string filePath)
		{
			switch (getJournalFileType(filePath))
			{
				case JournalFileType.Markdown:
					return MarkdownReader.fromFile(filePath);
			
				case JournalFileType.HTML:
					return File.ReadAllText(filePath);
			}

			throw new ArgumentException("invalid journal file type");
		}

		enum JournalFileType
		{
			Markdown,
			HTML
		}

		static bool isJournalFileType(string filename)
		{
			return null != getJournalFileType(filename);
		}

		static JournalFileType? getJournalFileType(string filename)
		{
			var ext = Path.GetExtension(filename);
			if (ext == null)
				return null;

			switch (ext.ToLowerInvariant())
			{
				case ".md":
					return JournalFileType.Markdown;
				case ".html":
				case ".htm":
					return JournalFileType.HTML;
			}

			return null;
		}
	}
}
