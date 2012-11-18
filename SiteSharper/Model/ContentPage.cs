namespace SiteSharper.Model
{
	sealed class ContentPage : Page
	{
		public ContentPage(string id, string name, string content)
			: base(id, name)
		{
			Content = content;
		}
	}
}
