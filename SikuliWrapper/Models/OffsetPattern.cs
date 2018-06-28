namespace SikuliWrapper.Models
{
	using System;
	using SikuliWrapper.Interfaces;

	public class OffsetPattern : IImage
	{
		private readonly IImage _pattern;
		private readonly Point _offset;

		public string Path => _pattern.Path;

		public OffsetPattern(IImage pattern, Point offset)
		{
			_pattern = pattern ?? throw new ArgumentNullException(nameof(pattern));
			_offset = offset;
		}

		public void Validate()
		{
			if(_pattern is OffsetPattern)
			{
				throw new Exception("Cannot use WithOffsetPattern with itself");
			}

			_pattern.Validate();
		}

		public string ToSikuliScript()
		{
			return $"{_pattern.ToSikuliScript()}.targetOffset({_offset.X}, {_offset.Y})";
		}
	}
}
