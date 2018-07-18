namespace SikuliWrapper.Interfaces
{
	public interface IImage
	{
		string Path { get; set; }
		string GeneratePatternString();
		string ToSikuliScript(string command, double commandParameter);
	}
}
