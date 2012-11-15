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
			var mod = new ModuleCall("CommentAreaID");
			mod
				.argument("account", _account)
				.argument("url", commentPageURL);

			return mod.toHTML();
		}

		public string createScriptForCommentLink(string commentPageURL)
		{
			var mod = new ModuleCall("CommentLinkID");
			mod
				.argument("account", _account)
				.argument("url", commentPageURL);

			return mod.toHTML();
		}
	}
}
