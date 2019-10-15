namespace SikuliWrapper
{
	using System;
	using System.Diagnostics;
	using System.Text.RegularExpressions;
	using SikuliWrapper.Exceptions;
	using SikuliWrapper.Interfaces;
	using SikuliWrapper.Utilities;

	public class SikuliRuntime : ISikuliRuntime
	{
		private Process _process;
		private IAsyncStreamsHandler _asyncStreamsHandler;
		private readonly ISikuliScriptProcessManager _sikuliScriptProcessManager;

		public SikuliRuntime(ISikuliScriptProcessManager sikuliScriptProcessManager)
		{
			_sikuliScriptProcessManager = sikuliScriptProcessManager ?? throw new ArgumentNullException(nameof(sikuliScriptProcessManager));
		}



































		public void Start()
		{
			if (_process != null)
			{
				throw new InvalidOperationException("This Sikuli session has already been started");
			}

			_process = _sikuliScriptProcessManager.Start("-i");
			_asyncStreamsHandler = new AsyncStreamsHandler(_process.StandardOutput, _process.StandardError, _process.StandardInput);
			_asyncStreamsHandler.ReadUntil(Constants.SikuliReadyTimeoutSeconds, Constants.InteractiveConsoleReadyMarker);
		}

		public void Stop(bool ignoreErrors = false)
		{
			if (_process == null)
			{
				return;
			}

			_asyncStreamsHandler.WriteLine(Constants.ExitCommand);

			if (!_process.HasExited)
			{
				if (!_process.WaitForExit(500))
				{
					_process.Kill();
				}
			}

			string errors = null;
			if (!ignoreErrors)
			{
				errors = _process.StandardError.ReadToEnd();
			}

			_asyncStreamsHandler.WaitForExit();

			_asyncStreamsHandler.Dispose();
			_asyncStreamsHandler = null;
			_process.Dispose();
			_process = null;

			if (!ignoreErrors && !string.IsNullOrEmpty(errors))
			{
				throw new SikuliException("Sikuli Errors: " + errors);
			}
		}

		public void Run(string command, double timeoutInSeconds = 1)
		{
			if (_process == null || _process.HasExited)
			{
				throw new InvalidOperationException("The Sikuli process is not running");
			}

			_asyncStreamsHandler.WriteLine(command);
			_asyncStreamsHandler.WriteLine("");
			_asyncStreamsHandler.WriteLine("");

			var result = _asyncStreamsHandler
				.ReadUntil(timeoutInSeconds, Constants.ErrorMarker, Constants.ResultPrefix);

			if (result.IndexOf(Constants.ErrorMarker, StringComparison.Ordinal) <= -1)
			{
				return;
			}

			result = result + Environment.NewLine + string.Join(Environment.NewLine, _asyncStreamsHandler.ReadUpToNow(0.1d));

			if (!result.Contains("FindFailed"))
			{
				throw new SikuliException(result);
			}

			var regex = new Regex("FindFailed: .+");
			throw new SikuliFindFailedException(regex.Match(result).Value);
		}

		public void Dispose()
		{
			Stop(true);
		}
	}
}
