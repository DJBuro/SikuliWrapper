namespace SikuliWrapper.Interfaces
{
	public interface IImage
	{
		string Path { get; }
		void Validate();
		string ToSikuliScript();
	}
}
