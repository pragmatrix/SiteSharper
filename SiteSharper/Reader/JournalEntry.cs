using System;
using System.Globalization;
using System.Text;

namespace SiteSharper.Reader
{
	public sealed class JournalEntry
	{
		public string Id;
		public JournalEntryFilename Filename;
		public string Content;
		public DateTime Date;

		public string ShortDate
		{
			get
			{
				return Date.ToString("dd MMM yyyy", CultureInfo.InvariantCulture);
			}
		}

		// http://stackoverflow.com/questions/284775/how-do-i-parse-and-convert-datetimes-to-the-rfc-822-date-time-format

		public string RFC822Date
		{
			get
			{
				var dto = new DateTimeOffset(Date);
				if (dto.Offset == TimeSpan.Zero)
					return dto.ToUniversalTime().ToString("ddd, dd MMM yyyy HH:mm:ss Z", CultureInfo.InvariantCulture);
				var builder = new StringBuilder(dto.ToString("ddd, dd MMM yyyy HH:mm:ss zzz", CultureInfo.InvariantCulture));
				builder.Remove(builder.Length - 3, 1);
				return builder.ToString();
			}
		}

		public string Title
		{
			get { return Filename.NamePart; }
		}
	}
}