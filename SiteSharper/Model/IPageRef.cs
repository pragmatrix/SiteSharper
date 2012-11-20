namespace SiteSharper.Model
{
	public interface IPageRef
	{
		string Id { get; }
		string Name { get; }
	}

	public static class PageRefExtensions
	{
		public static string getResourceFilePath(this IPageRef _)
		{
			var id = _.Id;
			return id.EndsWith("/") ? id.Substring(0, id.Length - 1) : id;
		}
	}
}
