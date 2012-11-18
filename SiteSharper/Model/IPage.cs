using SiteSharper.Writer;

namespace SiteSharper.Model
{
	public interface IPage : IPageRef
	{
		string Header { get; set; }
		// content can be set externally for now, so that we are able to 
		// load content based on the page's id. I am sure this has to be changed
		// later on.
		string Content { get; set; }

		void writePage(PageWriter writer, string html);
	}
}
