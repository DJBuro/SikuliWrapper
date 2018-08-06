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
		private Mock<ISikuliRuntime> _mockRuntime;
		private IImage _image;
		private int _invokeCount;

		[SetUp]
		public void SetUp()
		{
			_mockRuntime = new Mock<ISikuliRuntime>();
			_invokeCount = 0;
			_image = ImageFactory.FromFile(Path.GetFullPath(@"..\..\..\Utils\vs.png"));
		}

		[Test]
		public void InitilizeScreen_InvokeRuntimeStart_JustOnce()
		{
			var startInvokeCount = 0;
			_mockRuntime.Setup(r => r.Start()).Callback(() => startInvokeCount++);

			IScreen screen = new Screen(_mockRuntime.Object);

			startInvokeCount.Should().Be(1);
		}

		[Test]
		public void DisposeScreen_InvokeRuntimeStop_JustOnce()
		{
			_mockRuntime.Setup(r => r.Stop(It.IsAny<bool>())).Callback(() => _invokeCount++);

			IScreen screen = new Screen(_mockRuntime.Object);
			screen.Dispose();

			_invokeCount.Should().Be(1);
		}

		[Test]
		public void Click_InvokePattern_WithRightCommand()
		{
			var sikuliCommand = _image.ToSikuliScript("click", 0);
			_mockRuntime.Setup(r => r.Run(sikuliCommand, 0)).Callback(() => _invokeCount++);

			IScreen screen = new Screen(_mockRuntime.Object);
			screen.Click(_image);

			_invokeCount.Should().Be(1);
		}

		[Test]
		public void ClickWithOffset_InvokePattern_WithRightCommand()
		{
			var offset = new Point(5, 5);
			var offsetImage = new OffsetImage(_image, offset);
			var sikuliCommand = offsetImage.ToSikuliScript("click", 0);
			_mockRuntime.Setup(r => r.Run(sikuliCommand, 0)).Callback(() => _invokeCount++);

			IScreen screen = new Screen(_mockRuntime.Object);
			screen.Click(_image, offset);

			_invokeCount.Should().Be(1);
		}

		[Test]
		public void DoubleClick_InvokePattern_WithRightCommand()
		{
			var sikuliCommand = _image.ToSikuliScript("doubleClick", 0);
			_mockRuntime.Setup(r => r.Run(sikuliCommand, 0)).Callback(() => _invokeCount++);

			IScreen screen = new Screen(_mockRuntime.Object);
			screen.DoubleClick(_image);

			_invokeCount.Should().Be(1);
		}

		[Test]
		public void DoubleClickWithOffset_InvokePattern_WithRightCommand()
		{
			var offset = new Point(5, 5);
			var offsetImage = new OffsetImage(_image, offset);
			var sikuliCommand = offsetImage.ToSikuliScript("doubleClick", 0);
			_mockRuntime.Setup(r => r.Run(sikuliCommand, 0)).Callback(() => _invokeCount++);

			IScreen screen = new Screen(_mockRuntime.Object);
			screen.DoubleClick(_image, offset);

			_invokeCount.Should().Be(1);
		}

		[Test]
		public void WaitVanish_InvokePattern_WithRightCommand()
		{
			var sikuliCommand = _image.ToSikuliScript("waitVanish", 2);
			_mockRuntime.Setup(r => r.Run(sikuliCommand, It.IsAny<double>())).Callback(() => _invokeCount++);

			IScreen screen = new Screen(_mockRuntime.Object);
			screen.WaitVanish(_image, 2);

			_invokeCount.Should().Be(1);
		}

		[Test]
		public void Type_InvokePattern_WithRightCommand()
		{
			var script = "print \"SIKULI#: YES\" if type(\"test\") == 1 else \"SIKULI#: NO\"";
			_mockRuntime.Setup(r => r.Run(script, It.IsAny<double>())).Callback(() => _invokeCount++);

			IScreen screen = new Screen(_mockRuntime.Object);
			screen.Type(_image, "test");

			_invokeCount.Should().Be(1);
		}

		[Test]
		public void TypeWithInvalidString_InvokePattern_ShouldThrowException()
		{
			IScreen screen = new Screen(_mockRuntime.Object);
			Action action = () => screen.Type(_image, Environment.NewLine);

			action.Should()
				.Throw<ArgumentException>()
				.WithMessage(@"Text cannot contain control characters");
		}

		[Test]
		public void Hover_InvokePattern_WithRightCommand()
		{
			var sikuliCommand = _image.ToSikuliScript("hover", 0);
			_mockRuntime.Setup(r => r.Run(sikuliCommand, 0)).Callback(() => _invokeCount++);

			IScreen screen = new Screen(_mockRuntime.Object);
			screen.Hover(_image);

			_invokeCount.Should().Be(1);
		}

		[Test]
		public void HoverWithOffset_InvokePattern_WithRightCommand()
		{
			var offset = new Point(5, 5);
			var offsetImage = new OffsetImage(_image, offset);
			var sikuliCommand = offsetImage.ToSikuliScript("hover", 0);
			_mockRuntime.Setup(r => r.Run(sikuliCommand, 0)).Callback(() => _invokeCount++);

			IScreen screen = new Screen(_mockRuntime.Object);
			screen.Hover(_image, offset);

			_invokeCount.Should().Be(1);
		}

		[Test]
		public void RightClick_InvokePattern_WithRightCommand()
		{
			var sikuliCommand = _image.ToSikuliScript("rightClick", 0);
			_mockRuntime.Setup(r => r.Run(sikuliCommand, 0)).Callback(() => _invokeCount++);

			IScreen screen = new Screen(_mockRuntime.Object);
			screen.RightClick(_image);

			_invokeCount.Should().Be(1);
		}

		[Test]
		public void RightClickWithOffset_InvokePattern_WithRightCommand()
		{
			var offset = new Point(5, 5);
			var offsetImage = new OffsetImage(_image, offset);
			var sikuliCommand = offsetImage.ToSikuliScript("rightClick", 0);
			_mockRuntime.Setup(r => r.Run(sikuliCommand, 0)).Callback(() => _invokeCount++);

			IScreen screen = new Screen(_mockRuntime.Object);
			screen.RightClick(_image, offset);

			_invokeCount.Should().Be(1);
		}

		[Test]
		public void Exists_InvokePattern_WithRightCommand()
		{
			var sikuliCommand = _image.ToSikuliScript("exists", 1);
			_mockRuntime.Setup(r => r.Run(sikuliCommand, 1)).Callback(() => _invokeCount++);

			IScreen screen = new Screen(_mockRuntime.Object);
			screen.Exists(_image);

			_invokeCount.Should().Be(1);
		}

		[Test]
		public void Wait_InvokePattern_WithRightCommand()
		{
			var sikuliCommand = _image.ToSikuliScript("wait", 2);
			_mockRuntime.Setup(r => r.Run(sikuliCommand, 2)).Callback(() => _invokeCount++);

			IScreen screen = new Screen(_mockRuntime.Object);
			screen.Wait(_image);

			_invokeCount.Should().Be(1);
		}

		[Test]
		public void DragDrop_InvokePattern_WithRightCommand()
		{
			var sikuliCommand = $"print \"SIKULI#: YES\" if dragDrop({_image.GeneratePatternString()},{_image.GeneratePatternString()}0.0000 else \"SIKULI#: NO\"";
			_mockRuntime.Setup(r => r.Run(sikuliCommand, 0)).Callback(() => _invokeCount++);

			IScreen screen = new Screen(_mockRuntime.Object);
			screen.DragDrop(_image, _image);

			_invokeCount.Should().Be(1);
		}
	}
}
