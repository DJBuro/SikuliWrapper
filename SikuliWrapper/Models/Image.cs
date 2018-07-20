namespace SikuliWrapper.Models
{
	using System;
	using System.IO;

	public abstract class Image 
	{
		private string _path;

		protected Image(string path)
		{
			if (string.IsNullOrEmpty(path))
			{
				throw new ArgumentNullException(nameof(path));
			}

			Path = path;
		}

		public string Path
		{
			get => _path;
			set
			{
				ValidatePath(value);
				_path = value;
			}
		}

		protected string ToSukuliFloat(double timeoutInSeconds)
		{
			return timeoutInSeconds > 0.0 ? ", " + timeoutInSeconds.ToString("0.####") : "";
		}

		private void ValidatePath(string path)
		{
			if (!File.Exists(path))
			{
				throw new FileNotFoundException("Could not find image file specified in pattern: " + path, _path);
			}
		}
	}
}
