namespace SikuliWrapper.Models
{
	using System;
	using System.Globalization;
	using System.IO;
	using SikuliWrapper.Interfaces;

	public class FilePattern : IImage
	{
		private readonly string _path;
		private readonly double _similarity;

		public string Path => _path;

		public FilePattern(string path, double similarity)
		{
			if (similarity < 0 || similarity > 1)
			{
				throw new ArgumentOutOfRangeException(nameof(similarity), similarity, "similarity must be between 0 and 1");
			}

			_path = path ?? throw new ArgumentNullException(nameof(path));
			_similarity = similarity;
		}

		public void Validate()
		{
			if (!File.Exists(_path))
			{
				throw new FileNotFoundException("Could not find image file specified in pattern: " + _path, _path);
			}
		}

		public string ToSikuliScript()
		{
			return string.Format(NumberFormatInfo.InvariantInfo, "Pattern(\"{0}\").similar({1})", _path.Replace(@"\", @"\\"), _similarity);
		}
	}
}
