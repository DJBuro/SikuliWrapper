namespace SikuliWrapper.Utilities
{
	using SikuliWrapper.Interfaces;
	using SikuliWrapper.Models;

	public class ImageFactory
	{
		public const double DefaultSimilarity = 0.7;

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
