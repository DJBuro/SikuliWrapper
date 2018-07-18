namespace SikuliWrapper.Models
{
	using System;
	using System.Globalization;
	using SikuliWrapper.Interfaces;

	public class FilePattern : Pattern, IImage
	{
		private readonly double _similarity;

		public FilePattern(string path, double similarity) 
			: base (path)
		{
			if (similarity <= 0 || similarity >= 1)
			{
				throw new ArgumentOutOfRangeException(nameof(similarity));
			}

			_similarity = similarity;
		}
		
		public string ToSikuliScript(string command, double commandParameter)
		{
			return $"print \"SIKULI#: YES\" if {command}({GeneratePatternString()}{ToSukuliFloat(commandParameter)}) else \"SIKULI#: NO\"";
		}

		public string GeneratePatternString()
		{
			return string.Format(NumberFormatInfo.InvariantInfo, $"Pattern(\"{Path}\").similar({_similarity})");
		}
	}
}
