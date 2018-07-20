namespace SikuliWrapper.UnitTests
{
	using System;
	using System.IO;
	using FluentAssertions;
	using Moq;
	using NUnit.Framework;
	using SikuliWrapper.Interfaces;
	using SikuliWrapper.Models;
	using SikuliWrapper.Utilities;

	[TestFixture]
	public class ScreenTests
	{
		private Mock<ISikuliRuntime> mockRuntime;
		private IImage image;
		private int invokeCount;

		[SetUp]
		public void SetUp()
		{
			this.mockRuntime = new Mock<ISikuliRuntime>();
			invokeCount = 0;
			this.image = ImageFactory.FromFile(Path.GetFullPath(@"..\..\..\Utils\vs.png"));
		}

		[Test]
		public void InitilizeScreen_InvokeRuntimeStart_JustOnce()
		{
			int startInvokeCount = 0;
			mockRuntime.Setup(r => r.Start()).Callback(() => startInvokeCount++);

			IScreen screen = new Screen(mockRuntime.Object);

			startInvokeCount.Should().Be(1);
		}

		[Test]
		public void DisposeScreen_InvokeRuntimeStop_JustOnce()
		{
			mockRuntime.Setup(r => r.Stop(It.IsAny<bool>())).Callback(() => invokeCount++);

			IScreen screen = new Screen(mockRuntime.Object);
			screen.Dispose();

			invokeCount.Should().Be(1);
		}

		[Test]
		public void Click_InvokePattern_WithRightCommand()
		{
			var sikuliCommand = image.ToSikuliScript("click", 0);
			mockRuntime.Setup(r => r.Run(sikuliCommand, 0)).Callback(() => invokeCount++);

			IScreen screen = new Screen(mockRuntime.Object);
			screen.Click(image);

			invokeCount.Should().Be(1);
		}

		[Test]
		public void ClickWithOffset_InvokePattern_WithRightCommand()
		{
			Point offset = new Point(5, 5);
			var offsetImage = new OffsetImage(image, offset);
			var sikuliCommand = offsetImage.ToSikuliScript("click", 0);
			mockRuntime.Setup(r => r.Run(sikuliCommand, 0)).Callback(() => invokeCount++);

			IScreen screen = new Screen(mockRuntime.Object);
			screen.Click(image, offset);

			invokeCount.Should().Be(1);
		}

		[Test]
		public void DoubleClick_InvokePattern_WithRightCommand()
		{
			var sikuliCommand = image.ToSikuliScript("doubleClick", 0);
			mockRuntime.Setup(r => r.Run(sikuliCommand, 0)).Callback(() => invokeCount++);

			IScreen screen = new Screen(mockRuntime.Object);
			screen.DoubleClick(image);

			invokeCount.Should().Be(1);
		}

		[Test]
		public void DoubleClickWithOffset_InvokePattern_WithRightCommand()
		{
			Point offset = new Point(5, 5);
			var offsetImage = new OffsetImage(image, offset);
			var sikuliCommand = offsetImage.ToSikuliScript("doubleClick", 0);
			mockRuntime.Setup(r => r.Run(sikuliCommand, 0)).Callback(() => invokeCount++);

			IScreen screen = new Screen(mockRuntime.Object);
			screen.DoubleClick(image, offset);

			invokeCount.Should().Be(1);
		}

		[Test]
		public void WaitVanish_InvokePattern_WithRightCommand()
		{
			var sikuliCommand = image.ToSikuliScript("waitVanish", 2);
			mockRuntime.Setup(r => r.Run(sikuliCommand, It.IsAny<double>())).Callback(() => invokeCount++);

			IScreen screen = new Screen(mockRuntime.Object);
			screen.WaitVanish(image, 2);

			invokeCount.Should().Be(1);
		}

		[Test]
		public void Type_InvokePattern_WithRightCommand()
		{
			string script = $"print \"SIKULI#: YES\" if type(\"test\") == 1 else \"SIKULI#: NO\"";
			mockRuntime.Setup(r => r.Run(script, It.IsAny<double>())).Callback(() => invokeCount++);

			IScreen screen = new Screen(mockRuntime.Object);
			screen.Type(image, "test");

			invokeCount.Should().Be(1);
		}

		[Test]
		public void TypeWithInvalidString_InvokePattern_ShouldThrowException()
		{
			IScreen screen = new Screen(mockRuntime.Object);
			Action action = () => screen.Type(image, Environment.NewLine);

			action.Should()
				.Throw<ArgumentException>()
				.WithMessage(@"Text cannot contain control characters");
		}

		[Test]
		public void Hover_InvokePattern_WithRightCommand()
		{
			var sikuliCommand = image.ToSikuliScript("hover", 0);
			mockRuntime.Setup(r => r.Run(sikuliCommand, 0)).Callback(() => invokeCount++);

			IScreen screen = new Screen(mockRuntime.Object);
			screen.Hover(image);

			invokeCount.Should().Be(1);
		}

		[Test]
		public void HoverWithOffset_InvokePattern_WithRightCommand()
		{
			Point offset = new Point(5, 5);
			var offsetImage = new OffsetImage(image, offset);
			var sikuliCommand = offsetImage.ToSikuliScript("hover", 0);
			mockRuntime.Setup(r => r.Run(sikuliCommand, 0)).Callback(() => invokeCount++);

			IScreen screen = new Screen(mockRuntime.Object);
			screen.Hover(image, offset);

			invokeCount.Should().Be(1);
		}

		[Test]
		public void RightClick_InvokePattern_WithRightCommand()
		{
			var sikuliCommand = image.ToSikuliScript("rightClick", 0);
			mockRuntime.Setup(r => r.Run(sikuliCommand, 0)).Callback(() => invokeCount++);

			IScreen screen = new Screen(mockRuntime.Object);
			screen.RightClick(image);

			invokeCount.Should().Be(1);
		}

		[Test]
		public void RightClickWithOffset_InvokePattern_WithRightCommand()
		{
			Point offset = new Point(5, 5);
			var offsetImage = new OffsetImage(image, offset);
			var sikuliCommand = offsetImage.ToSikuliScript("rightClick", 0);
			mockRuntime.Setup(r => r.Run(sikuliCommand, 0)).Callback(() => invokeCount++);

			IScreen screen = new Screen(mockRuntime.Object);
			screen.RightClick(image, offset);

			invokeCount.Should().Be(1);
		}

		[Test]
		public void Exists_InvokePattern_WithRightCommand()
		{
			var sikuliCommand = image.ToSikuliScript("exists", 1);
			mockRuntime.Setup(r => r.Run(sikuliCommand, 1)).Callback(() => invokeCount++);

			IScreen screen = new Screen(mockRuntime.Object);
			screen.Exists(image);

			invokeCount.Should().Be(1);
		}

		[Test]
		public void Wait_InvokePattern_WithRightCommand()
		{
			var sikuliCommand = image.ToSikuliScript("wait", 2);
			mockRuntime.Setup(r => r.Run(sikuliCommand, 2)).Callback(() => invokeCount++);

			IScreen screen = new Screen(mockRuntime.Object);
			screen.Wait(image);

			invokeCount.Should().Be(1);
		}

		[Test]
		public void DragDrop_InvokePattern_WithRightCommand()
		{
			string sikuliCommand = $"print \"SIKULI#: YES\" if dragDrop({image.GeneratePatternString()},{image.GeneratePatternString()}0.0000 else \"SIKULI#: NO\"";
			mockRuntime.Setup(r => r.Run(sikuliCommand, 0)).Callback(() => invokeCount++);

			IScreen screen = new Screen(mockRuntime.Object);
			screen.DragDrop(image, image);

			invokeCount.Should().Be(1);
		}
	}
}
