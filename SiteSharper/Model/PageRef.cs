namespace SiteSharper.Model
{
	public sealed class PageRef : IPageRef, IHasURL
	{
		public PageRef(string id, string name, string url)
		{
			Id = id;
			Name = name;
			URL = url;
		}

		public string Id { get; private set; }
		public string Name { get; private set; }
		public string URL { get; private set; }
	}
}
