namespace SikuliWrapper.Exceptions
{
	using System;

	public class SikuliException : Exception
	{
		public SikuliException(string message)
			: base(message)
		{
		}
	}
}
