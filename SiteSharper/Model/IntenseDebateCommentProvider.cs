using SiteSharper.Reader;

namespace SiteSharper.Model
{
	public sealed class IntenseDebateCommentProvider : ICommentProvider
	{
		readonly string _account;

		public IntenseDebateCommentProvider(string account)
		{
			_account = account;
		}

		public string createScriptForCommentArea(string commentPageURL)
		{
			var id = createIdFromURL(commentPageURL);

			var mod = new ModuleCall("CommentAreaID");
			mod
				.argument("account", _account)
				.argument("id", id)
				.argument("url", commentPageURL);

			return mod.toHTML();
		}

		public string createScriptForCommentLink(string commentPageURL)
		{
			// note: id is just to discriminate the situation in which multiple links appear on one page.
			// sadly the php script has parts in it that is replaced with this id, so the id must comply to 
			// the format of php variable names. ReadableURL is a good choice in that case I guess.
			// Also note that the ID term of an post-id is misleading here, the posts seem to be always 
			// discriminated based on their urls.

			var id = createIdFromURL(commentPageURL);

			var mod = new ModuleCall("CommentLinkID");
			mod
				.argument("account", _account)
				.argument("url", commentPageURL)
				.argument("id", id);

			return mod.toHTML();
		}

		static string createIdFromURL(string commentPageURL)
		{
			return ReadableURL.read(commentPageURL)
				.Replace("-", "_");
		}
	}
}
