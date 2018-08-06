namespace SikuliWrapper.Interfaces
{
	using System;
	using System.Collections.Generic;

	public interface IAsyncStreamsHandler : IDisposable
	{
		string ReadUntil(double timeoutInSeconds, params string[] expectedStrings);
		IEnumerable<string> ReadUpToNow(double timeoutInSeconds);
		void WriteLine(string command);
		void WaitForExit();
	}
}
