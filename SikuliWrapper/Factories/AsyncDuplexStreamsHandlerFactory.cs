namespace SikuliWrapper.Factories
{
	using System.IO;
	using SikuliWrapper.Interfaces;

	public class AsyncDuplexStreamsHandlerFactory : IAsyncStreamsHandlerFactory
	{
		public IAsyncStreamsHandler Create(TextReader stdout, TextReader stderr, TextWriter stdin)
		{
			return new AsyncStreamsHandler(stdout, stderr, stdin);
		}
	}
}
