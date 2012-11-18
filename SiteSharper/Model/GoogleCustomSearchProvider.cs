namespace SiteSharper.Model
{
	public sealed class GoogleCustomSearchProvider : ISearchProvider
	{
		public GoogleCustomSearchProvider(string uniqueId)
		{
			var xx = new ModuleCall("GoogleCustomSearchHeader")
				.argument("id", uniqueId);

			Header = xx.toHTML();
			Inline = "<gcse:search xmlns:gcse=\"/\"></gcse:search>";
		}

		public string Header { get; private set; }
		public string Inline { get; private set; }
	}
}
