namespace SikuliWrapper.Exceptions
{
	using System.Diagnostics.CodeAnalysis;

	[ExcludeFromCodeCoverage]
	public class SikuliFindFailedException : SikuliException
	{
		public SikuliFindFailedException(string message)
			: base(message)
		{
		}
	}
}
