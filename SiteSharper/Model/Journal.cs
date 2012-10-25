namespace SiteSharper.Model
{
	/*
		A journal that refers to content and resource at a filesystem location.
	*/

	public sealed class Journal
	{
		public readonly string Id;
		public readonly string Path;

		public Journal(string id, string path)
		{
			Id = id;
			Path = path;
		}
	}
}
