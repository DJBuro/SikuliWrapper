namespace SikuliWrapper.UnitTests
{
	using System;
	using System.Diagnostics;
	using System.Linq;
	using System.Reflection;
	using FluentAssertions;
	using Moq;
	using NUnit.Framework;
	using SikuliWrapper.Interfaces;
	using SikuliWrapper.Utilities;

	[TestFixture]
	public class SikuliRuntimeTests
	{
		private ISikuliScriptProcessManager _manager;

		[SetUp]
		public void SetUp()
		{
			_manager = new SikuliScriptProcessManager();
		}

		[Test]
		public void SikuliRunTime_WithNullManager_ShoulThrowException()
		{
			Action action = () => new SikuliRuntime(null);

			action.Should().Throw<ArgumentNullException>()
				.WithMessage("Value cannot be null.\r\nParameter name: sikuliScriptProcessManager");
		}
		
		[Test]
		public void Start_WithNotNullProcess_ShoulThrowException()
		{
			ISikuliRuntime runtime = new SikuliRuntime(_manager);
			var process = runtime
				.GetType()
				.GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
				.FirstOrDefault(f => f.FieldType == typeof(Process));

			var processMock = new Mock<Process>();
			process.SetValue(runtime, processMock.Object);

			runtime.Invoking(m => m.Start()).Should().Throw<InvalidOperationException>();
		}

		[Test]
		public void Stop_WithNullProcess_ShouldReturn()
		{
			var runtime = new SikuliRuntime(_manager);
			runtime.Invoking(m => m.Stop()).Should().NotThrow();
		}
	}
}