namespace SiteSharper.Model
{
	public sealed class Resource
	{
		public Resource(string sourcePath, string relativeTargetPath)
		{
			SourcePath = sourcePath;
			RelativeTargetPath = relativeTargetPath;
		}

		public readonly string SourcePath;
		public readonly string RelativeTargetPath;
	}
}
