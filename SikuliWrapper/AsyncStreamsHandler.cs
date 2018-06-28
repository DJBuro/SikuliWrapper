namespace SikuliWrapper
{
	using System;
	using System.Collections.Concurrent;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Threading.Tasks;

	using SikuliWrapper.Interfaces;

	public class AsyncStreamsHandler : IAsyncStreamsHandler
	{
		private readonly TextReader _stdout;
		private readonly TextReader _stderr;
		private readonly TextWriter _stdin;
		private readonly Task _readStderrTask;
		private readonly Task _readStdoutTask;
		private readonly BlockingCollection<string> _pendingOutputLines = new BlockingCollection<string>();

		public AsyncStreamsHandler(TextReader stdout, TextReader stderr, TextWriter stdin)
		{
			_stdout = stdout;
			_stderr = stderr;
			_stdin = stdin;

			_readStdoutTask = Task.Factory.StartNew(() => ReadStdout(), TaskCreationOptions.LongRunning);
			_readStderrTask = Task.Factory.StartNew(() => ReadStderr(), TaskCreationOptions.LongRunning);
		}

		public string ReadUntil(double timeoutInSeconds, params string[] expectedStrings)
		{
			while (true)
			{
				string line;
				if (timeoutInSeconds > 0)
				{
					TimeSpan timeout = TimeSpan.FromSeconds(timeoutInSeconds);
					if (!_pendingOutputLines.TryTake(out line, timeout))
					{
						throw new TimeoutException($@"No result in alloted time: {timeout}");
					}
				}
				else
				{
					line = _pendingOutputLines.Take();
				}

				if (expectedStrings.Any(s => line.IndexOf(s, StringComparison.Ordinal) > -1))
				{
					return line;
				}
			}
		}

		public IEnumerable<string> ReadUpToNow(double timeoutInSeconds)
		{
			while (true)
			{
				string line;
				if (_pendingOutputLines.TryTake(out line, TimeSpan.FromSeconds(timeoutInSeconds)))
				{
					yield return line;
				}
				else
				{
					yield break;
				}
			}
		}

		public void WriteLine(string command)
		{
			_stdin.WriteLine(command);
			File.AppendAllText(@"..\..\..\input.txt", command + Environment.NewLine);
		}

		public void WaitForExit()
		{
			_readStdoutTask.Wait();
			_readStderrTask.Wait();
		}

		public void Dispose()
		{
			if (_readStdoutTask != null)
			{
				_readStdoutTask.Dispose();
			}

			if (_readStderrTask != null)
			{
				_readStderrTask.Dispose();
			}
		}

		private void ReadStdout()
		{
			ReadStd(_stdout);
		}

		private void ReadStderr()
		{
			ReadStd(_stderr, "[error] ");
		}

		private void ReadStd(TextReader output, string prefix = null)
		{
			string line;
			while ((line = output.ReadLine()) != null)
			{
				if (prefix != null)
				{
					line = prefix + line;
				}

				File.AppendAllText(@"..\..\..\log.txt", line + Environment.NewLine);
				_pendingOutputLines.Add(line);
			}
		}
	}
}
