namespace SikuliWrapper.Interfaces
{
	using System;

	public interface ISikuliRuntime : IDisposable
	{
		void Start();

		void Stop(bool ignoreErrors = false);

		void Run(string command, double timeoutInSeconds = 1);
	}
}
