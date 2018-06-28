﻿namespace SikuliWrapper
{
	using System;
	using System.Diagnostics;

	using SikuliWrapper.Exceptions;
	using SikuliWrapper.Interfaces;

	public class SikuliRuntime : ISikuliRuntime
	{
		private readonly IAsyncStreamsHandlerFactory _asyncDuplexStreamsHandlerFactory;
		private Process _process;
		private IAsyncStreamsHandler _asyncTwoWayStreamsHandler;
		private readonly ISikuliScriptProcessFactory _sikuliScriptProcessFactory;

		private const string InteractiveConsoleReadyMarker = "... use ctrl-d to end the session";
		private const string ErrorMarker = "[error]";
		private const string ExitCommand = "exit()";
		private const int SikuliReadyTimeoutSeconds = 40;

		public SikuliRuntime(IAsyncStreamsHandlerFactory asyncDuplexStreamsHandlerFactory, ISikuliScriptProcessFactory sikuliScriptProcessFactory)
		{
			if (asyncDuplexStreamsHandlerFactory == null)
			{
				throw new ArgumentNullException(nameof(asyncDuplexStreamsHandlerFactory));
			}

			if (sikuliScriptProcessFactory == null)
			{
				throw new ArgumentNullException(nameof(sikuliScriptProcessFactory));
			}

			_asyncDuplexStreamsHandlerFactory = asyncDuplexStreamsHandlerFactory;
			_sikuliScriptProcessFactory = sikuliScriptProcessFactory;
		}

		public void Start()
		{
			if (_process != null)
            {
                throw new InvalidOperationException("This Sikuli session has already been started");
            }

            _process = _sikuliScriptProcessFactory.Start("-i");

			_asyncTwoWayStreamsHandler = _asyncDuplexStreamsHandlerFactory.Create(_process.StandardOutput, _process.StandardError, _process.StandardInput);
			_asyncTwoWayStreamsHandler.ReadUntil(SikuliReadyTimeoutSeconds, InteractiveConsoleReadyMarker);
		}

		public void Stop(bool ignoreErrors = false)
		{
			if (_process == null) return;

			_asyncTwoWayStreamsHandler.WriteLine(ExitCommand);

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

			_asyncTwoWayStreamsHandler.WaitForExit();

			_asyncTwoWayStreamsHandler.Dispose();
			_asyncTwoWayStreamsHandler = null;
			_process.Dispose();
			_process = null;

			if (!ignoreErrors && !String.IsNullOrEmpty(errors))
			{
				throw new SikuliException("Sikuli Errors: " + errors);
			}
		}

		public string Run(string command, string resultPrefix, double timeoutInSeconds)
		{
			if (_process == null || _process.HasExited)
			{
				throw new InvalidOperationException("The Sikuli process is not running");
			}

			_asyncTwoWayStreamsHandler.WriteLine(command);
			_asyncTwoWayStreamsHandler.WriteLine("");
			_asyncTwoWayStreamsHandler.WriteLine("");

			string result = _asyncTwoWayStreamsHandler.ReadUntil(timeoutInSeconds, ErrorMarker, resultPrefix);

			if (result.IndexOf(ErrorMarker, StringComparison.Ordinal) > -1)
			{
				result = result + Environment.NewLine + string.Join(Environment.NewLine, _asyncTwoWayStreamsHandler.ReadUpToNow(0.1d));
				if (result.Contains("FindFailed"))
				{
					throw new SikuliFindFailedException(result);
				}
				throw new SikuliException(result);
			}

			return result;
		}

		public void Dispose()
		{
			Stop(true);
		}
	}
}
