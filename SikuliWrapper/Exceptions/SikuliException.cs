namespace SikuliWrapper.Exceptions
{
	using System;
	using System.Diagnostics.CodeAnalysis;

	[ExcludeFromCodeCoverage]
	public class SikuliException : Exception
	{
		public SikuliException(string message)
			: base(message)
		{
		}
	}
}
