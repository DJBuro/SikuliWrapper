namespace SikuliWrapper.Exceptions
{
	using System;

	public class PatternException : Exception
    {
		public PatternException(string message, Exception innerException)
			:base(message, innerException)
		{

		}
    }
}
