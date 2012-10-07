using System.Text;
using System.Web;

namespace ProductSite.TemplateGenerator
{
	public abstract class RazorHtmlTemplate
	{
		public abstract string generate(object model);
	}

	public abstract class RazorHtmlTemplate<ModelT> : RazorHtmlTemplate
	{
		readonly StringBuilder _sb = new StringBuilder();

		public override string generate(object model)
		{
			_sb.Clear();
			Model = (ModelT)model;
			Execute();
			return _sb.ToString();
		}

		public abstract void Execute();

		protected ModelT Model;

		protected void Write(object obj)
		{
			if (obj == null)
			{
				WriteLiteral("not set");
				return;
			}
			WriteLiteral(HttpUtility.HtmlEncode(obj.ToString()));
		}

		protected void WriteLiteral(string literal)
		{
			_sb.Append(literal);
		}
	}
}
