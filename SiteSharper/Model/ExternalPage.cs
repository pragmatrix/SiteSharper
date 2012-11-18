namespace SiteSharper.Model
{
	public sealed class ExternalPage : IPageRef, IHasURL
	{
		public ExternalPage(string id, string name, string url)
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
