namespace SikuliWrapper.Interfaces
{
	using System;

	public interface ISikuliRuntime : IDisposable
	{
		void Start();
		void Stop(bool ignoreErrors = false);
		string Run(string command, string resultPrefix, double timeoutInSeconds);
	}
}
