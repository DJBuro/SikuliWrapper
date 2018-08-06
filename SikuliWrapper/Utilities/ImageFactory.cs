namespace SikuliWrapper.Utilities
{
	using SikuliWrapper.Interfaces;
	using SikuliWrapper.Models;

	public static class ImageFactory
	{
		private const double DefaultSimilarity = 0.7;

		public static IImage FromFile(string path, double similarity = DefaultSimilarity)
		{
			return new FileImage(path, similarity);
		}

		public static Location Location(int x, int y)
		{
			return new Location(x, y);
		}
	}
}
