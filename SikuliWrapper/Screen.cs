namespace SikuliWrapper
{
	using System;
	using System.IO;
	using System.Text.RegularExpressions;
	using SikuliWrapper.Interfaces;
	using SikuliWrapper.Models;
	using SikuliWrapper.Utilities;

	public class Screen : IScreen
	{
		private static readonly Regex InvalidTextRegex = new Regex(@"[\r\n\t\x00-\x1F]", RegexOptions.Compiled);
		private readonly ISikuliRuntime _runtime;

		public Screen()
		{
			var manager = new SikuliScriptProcessManager();

			_runtime = new SikuliRuntime(manager);
			_runtime.Start();
		}

		public Screen(ISikuliRuntime sikuliRuntime)
		{
			_runtime = sikuliRuntime;
			_runtime.Start();
		}

		public void Exists(IImage image, double timeoutInSeconds = 1)
		{
			_runtime.Run(image.ToSikuliScript("exists", timeoutInSeconds), timeoutInSeconds);
		}

		public void Click(IImage image)
		{
			_runtime.Run(image.ToSikuliScript("click", 0));
		}

		public void Click(IImage image, Point offset)
		{
			_runtime.Run(new OffsetPattern(image, offset).ToSikuliScript("click", 0));
		}

		public void DoubleClick(IImage image)
		{
			_runtime.Run(image.ToSikuliScript("doubleClick", 0));
		}

		public void DoubleClick(IImage image, Point offset)
		{
			_runtime.Run(new OffsetPattern(image, offset).ToSikuliScript("doubleClick", 0));
		}

		public void Wait(IImage image, double timeoutInSeconds = 2)
		{
			_runtime.Run(image.ToSikuliScript("wait", timeoutInSeconds), timeoutInSeconds);
		}

		public void WaitVanish(IImage image, double timeoutInSeconds = 1)
		{
			_runtime.Run(image.ToSikuliScript("waitVanish", timeoutInSeconds));
		}

		public void Type(IImage image, string text)
		{
			Click(image);
			if (InvalidTextRegex.IsMatch(text))
			{
				throw new ArgumentException("Text cannot contain control characters");
			}

			string script = $"print \"SIKULI#: YES\" if type(\"{text}\") == 1 else \"SIKULI#: NO\"";
			_runtime.Run(script);
		}

		public void Hover(IImage image)
		{
			_runtime.Run(image.ToSikuliScript("hover", 0));
		}

		public void Hover(IImage image, Point offset)
		{
			_runtime.Run(new OffsetPattern(image, offset).ToSikuliScript("hover", 0));
		}

		public void RightClick(IImage image)
		{
			_runtime.Run(image.ToSikuliScript("rightClick", 0));
		}

		public void RightClick(IImage image, Point offset)
		{
			_runtime.Run(new OffsetPattern(image, offset).ToSikuliScript("rightClick", 0));
		}

		public void DragDrop(IImage fromImage, IImage toImage)
		{
			string script = $"print \"SIKULI#: YES\" if dragDrop({fromImage.GeneratePatternString()},{toImage.GeneratePatternString()}0.0000 else \"SIKULI#: NO\"";
			_runtime.Run(script); // Failsafe
		}

		public void Dispose()
		{
			_runtime.Stop();
		}
	}
}
