namespace SikuliWrapper.Interfaces
{
	using System.IO;

	public interface IAsyncStreamsHandlerFactory
	{
		IAsyncStreamsHandler Create(TextReader stdout, TextReader stderr, TextWriter stdin);
	}
}
