namespace SiteSharper.Model
{
	public interface ICommentProvider
	{
		// note the comment area can only be placed on the same page as the commentPageURL!
		string createScriptForCommentArea(string commentPageURL);
		string createScriptForCommentLink(string commentPageURL);
	}
}
