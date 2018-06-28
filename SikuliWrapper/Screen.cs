namespace SikuliWrapper
{
	using System;
	using System.Text.RegularExpressions;
	using SikuliWrapper.Exceptions;
	using SikuliWrapper.Interfaces;
	using SikuliWrapper.Models;

	public class Screen : IScreen
	{
		private static readonly Regex InvalidTextRegex = new Regex(@"[\r\n\t\x00-\x1F]", RegexOptions.Compiled);
		private readonly ISikuliRuntime _runtime;

		public Screen(ISikuliRuntime sikuliRuntime)
		{
			_runtime = sikuliRuntime;
			_runtime.Start();
		}

		public bool Exists(IImage pattern, double timeoutInSeconds = 0)
		{
			return RunCommand("exists", pattern, timeoutInSeconds);
		}

		public bool Click(IImage pattern)
		{
			return RunCommand("click", pattern, 0);
		}

		public bool Click(IImage pattern, Point offset)
		{
			return RunCommand("click", new OffsetPattern(pattern, offset), 0);
		}

		public bool DoubleClick(IImage pattern)
		{
			return RunCommand("doubleClick", pattern, 0);
		}

		public bool DoubleClick(IImage pattern, Point offset)
		{
			return RunCommand("doubleClick", new OffsetPattern(pattern, offset), 0);
		}

		public bool Wait(IImage pattern, double timeoutInSeconds = 2)
		{
			return RunCommand("wait", pattern, timeoutInSeconds);
		}

		public bool WaitVanish(IImage pattern, double timeoutInSeconds = 0)
		{
			return RunCommand("waitVanish", pattern, timeoutInSeconds);
		}

		public bool Type(IImage image, string text)
		{
			Click(image);
			if (InvalidTextRegex.IsMatch(text))
			{
				throw new ArgumentException("Text cannot contain control characters. Escape them before, e.g. \\n should be \\\\n", nameof(text));
			}

			string script = $"print \"SIKULI#: YES\" if type(\"{text}\") == 1 else \"SIKULI#: NO\"";
			string result = _runtime.Run(script, "SIKULI#: ", 0d);
			return result.Contains("SIKULI#: YES");
		}

		public bool Hover(IImage pattern)
		{
			return RunCommand("hover", pattern, 0);
		}

		public bool Hover(IImage pattern, Point offset)
		{
			return RunCommand("hover", new OffsetPattern(pattern, offset), 0);
		}

		public bool RightClick(IImage pattern)
		{
			return RunCommand("rightClick", pattern, 0);
		}

		public bool RightClick(IImage pattern, Point offset)
		{
			return RunCommand("rightClick", new OffsetPattern(pattern, offset), 0);
		}

		public bool DragDrop(IImage fromPattern, IImage toPattern)
		{
			return RunCommand("dragDrop", fromPattern, toPattern, 0);
		}

		private bool RunCommand(string command, IImage pattern, double commandParameter)
		{
			try
			{
				pattern.Validate();

				string script = $"print \"SIKULI#: YES\" if {command}({pattern.ToSikuliScript()}{ToSukuliFloat(commandParameter)}) else \"SIKULI#: NO\"";

				string result = _runtime.Run(script, "SIKULI#: ", commandParameter); // Failsafe
				return result.Contains("SIKULI#: YES");
			}
			catch (Exception ex)
			{
				throw new PatternException($"Element can not be found on screen {pattern.Path} ", ex);
			}
		}

		private bool RunCommand(string command, IImage fromPattern, IImage toPattern, float commandParameter)
		{
			fromPattern.Validate();
			toPattern.Validate();

			string script = $"print \"SIKULI#: YES\" if {command}({fromPattern.ToSikuliScript()},{toPattern.ToSikuliScript()}{ToSukuliFloat(commandParameter)}) else \"SIKULI#: NO\"";
			string result = _runtime.Run(script, "SIKULI#: ", commandParameter * 1.5d); // Failsafe
			return result.Contains("SIKULI#: YES");
		}
		
		private static string ToSukuliFloat(double timeoutInSeconds)
		{
			return timeoutInSeconds > 0f ? ", " + timeoutInSeconds.ToString("0.####") : "";
		}

		public void Dispose()
		{
			_runtime.Stop();
		}
	}
}
