using System.Collections.Generic;

namespace SiteSharper.Model
{
	public sealed class ExternalPage : IPageRef, IHasURL, ISpecifiesReferenceClasses
	{
		public ExternalPage(string id, string name, string url)
		{
			Id = id;
			Name = name;
			URL = url;
			ReferenceClasses = new List<string>();
		}


		public string Id { get; private set; }
		public string Name { get; private set; }
		public string URL { get; private set; }
		public List<string> ReferenceClasses { get; private set;}
	}
}
