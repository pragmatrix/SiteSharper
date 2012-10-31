﻿using System;

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

		public FeedSettings FeedSettings = FeedSettings.Default;

		public Journal(string id, string path, string title, string description = "")
		{
			Id = id;
			Path = path;
			Title = title;
			Description = description;
		}

		public string FeedSitePath
		{
			get { return Id + "/feed"; }
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
