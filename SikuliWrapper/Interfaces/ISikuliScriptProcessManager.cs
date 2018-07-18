namespace SikuliWrapper.Interfaces
{
	using System.Diagnostics;

	public interface ISikuliScriptProcessManager
	{
		Process Start(string args);
	}
}
