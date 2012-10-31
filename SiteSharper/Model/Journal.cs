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

		public Journal(string id, string path, string title, string description = "")
		{
			Id = id;
			Path = path;
			Title = title;
			Description = description;
		}

		public string SitePath
		{
			get { return Id + "/feed"; }
		}
	}
}
