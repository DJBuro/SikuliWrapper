namespace SikuliWrapper.Interfaces
{
	using System.Diagnostics;

	public interface ISikuliScriptProcessFactory
	{
		Process Start(string args);
	}
}
