using System;

namespace SiteSharper.Model
{
	/*
		A journal that refers to content and resource at a filesystem location.
	*/

	public sealed class Journal
	{
		public readonly string Id;
		public readonly string Path;

		public readonly string Title;
		public readonly string Description;

		public ICommentProvider Comments_;

		public FeedSettings FeedSettings = FeedSettings.Default;

		public Journal(string id, string path, string title, string description = "")
		{
			Id = id;
			Path = path;
			Title = title;
			Description = description;
		}

		public Journal enableComments(ICommentProvider provider)
		{
			Comments_ = provider;
			return this;
		}

		public string FeedSitePath
		{
			get { return Id + "/feed"; }
		}

		public IPageRef indexReference(string title)
		{
			var url = "/" + Id + "/index";
			return new PageRef(Id + "/index", title, url);
		}
	}

	public struct FeedSettings
	{
		public uint MinimumNumberOfItems;
		public TimeSpan MinimumTimeSpanToCover;

		public static FeedSettings Default = new FeedSettings
		{
			MinimumNumberOfItems = 10,
			MinimumTimeSpanToCover = TimeSpan.FromDays(365 / 4)
		};
	}
}
