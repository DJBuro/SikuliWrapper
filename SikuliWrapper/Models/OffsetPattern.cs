namespace SikuliWrapper.Models
{
	using System;
	using SikuliWrapper.Interfaces;

	public class OffsetPattern : Pattern, IImage
	{
		private readonly IImage _pattern;
		private readonly Point _offset;
		
		public OffsetPattern(IImage pattern, Point offset)
			: base (pattern.Path)
		{
			_pattern = pattern;
			_offset = offset;
		}

		public string ToSikuliScript(string command, double commandParameter)
		{
			return
				$"print \"SIKULI#: YES\" if {command}({GeneratePatternString()}{ToSukuliFloat(commandParameter)}) else \"SIKULI#: NO\"";
		}

		public string GeneratePatternString()
		{
			return $"{_pattern.GeneratePatternString()}.targetOffset({_offset.X}, {_offset.Y})";
		}
	}
}
