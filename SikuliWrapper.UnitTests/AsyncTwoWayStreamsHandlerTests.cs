namespace SikuliWrapper.UnitTests
{
	using System;
	using System.Diagnostics;
	using System.IO;
	using System.Text;
	using System.Threading;
	using System.Threading.Tasks;
	using FluentAssertions;
	using NUnit.Framework;
	using SikuliWrapper.UnitTests.Utils;

	[TestFixture]
	public class AsyncTwoWayStreamsHandlerTests
	{
		[Test]
		public void CanWrite()
		{
			var stdout = new StreamReader(new MemoryStream());
			var sb = new StringBuilder();
			var stdin = new StringWriter(sb);

			var handler = new AsyncStreamsHandler(stdout, new StringReader(""), stdin);

			handler.WriteLine("Should end up in the StreamWriter");
			handler.WriteLine("With a line break after each");

			var actualResult = sb.ToString();
			actualResult.Should().Be("Should end up in the StreamWriter" +
									  Environment.NewLine +
									  "With a line break after each" +
									  Environment.NewLine);
		}

		[Test]
		public async Task ReadUntil_StringIsFound()
		{
			var stdout = new StringReader(String.Join(Environment.NewLine,
				"This line should be ignored because it comes before",
				"This line should be taken because it includes MARKER in it",
				"This line should be ignored because it comes after") + Environment.NewLine);
			var stdin = new StringWriter();
			var handler = new AsyncStreamsHandler(stdout, new StringReader(""), stdin);

			var actualResult = handler.ReadUntil(0, "MARKER");

			actualResult.Should().Be("This line should be taken because it includes MARKER in it");
		}

		[Test]
		public async Task ReadUntil_StringIsFoundInError()
		{
			var stdout = new StringReader("This line should be ignored" + Environment.NewLine);
			var stderr = new StringReader("This line should be taken because it includes MARKER in it" + Environment.NewLine);
			var stdin = new StringWriter();
			var handler = new AsyncStreamsHandler(stdout, stderr, stdin);

			string actualResult = handler.ReadUntil(0, "MARKER");

			actualResult.Should().Be("[error] This line should be taken because it includes MARKER in it");
		}

		[Test]
		public async Task ReadUntilTimeoutThrows()
		{
			var stream = new BlockingStream();
			var stdout = new StreamReader(stream);
			var stdin = new StringWriter();

			var handler = new AsyncStreamsHandler(stdout, new StringReader(""), stdin);

			handler.Invoking(h => h.ReadUntil(0.1, "MARKER"))
				.Should()
				.Throw<TimeoutException>()
				.WithMessage("No result in alloted time: 00:00:00.1000000");
		}

		[Test]
		public void WaitForExitExitsImmediatelyWhenNothingToWaitFor()
		{
			var stdout = new StringReader("");
			var stdin = new StringWriter();

			var handler = new AsyncStreamsHandler(stdout, new StringReader(""), stdin);

			handler.WaitForExit();
			Assert.Pass("Successfully skipped waiting since there was nothing to do");
		}

		[Test]
		public void WaitForExitBlockstdinhenStillReading()
		{
			var blockingStream = new BlockingStream();
			var stdout = new StreamReader(blockingStream);
			var stdin = new StringWriter();
			var handler = new AsyncStreamsHandler(stdout, new StringReader(""), stdin);
			var stopWatch = new Stopwatch();

			stopWatch.Start();
			new Task(() => { Thread.Sleep(100); blockingStream.Unblock(); }).Start();
			handler.WaitForExit();
			stopWatch.Stop();

			if (stopWatch.ElapsedMilliseconds < 100)
				Assert.Fail("WaitForExit returned before the timer finished");
			else
				Assert.Pass("Successfully waited for the stream to end");
		}
	}
}