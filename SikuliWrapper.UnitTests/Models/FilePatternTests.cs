namespace SikuliWrapper.UnitTests.Models
{
	using System;
	using System.IO;
	using FluentAssertions;
	using NUnit.Framework;
	using SikuliWrapper.Models;

	[TestFixture]
	public class FilePatternTests
	{
		[Test]
		[TestCase(null)]
		[TestCase("")]
		public void Create_FilePattern_WithNullOrEmptyString_ThrowException(string path)
		{
			Action action = () => new FileImage(path, 0.5);

			action.Should()
				.Throw<ArgumentNullException>()
				.WithMessage("Value cannot be null." + Environment.NewLine + "Parameter name: path");
		}

		[Test]
		[TestCase(double.MinValue)]
		[TestCase(double.MaxValue)]
		[TestCase(-0)]
		[TestCase(1)]
		public void CreateFilePattern_WithInvalidSimilarity_ThrowException(double similarity)
		{
			var pathToTestPic = Path.GetFullPath(@"..\..\..\Utils\vs.png");
			Action action = () => new FileImage(pathToTestPic, similarity);

			action.Should()
				.Throw<ArgumentOutOfRangeException>()
				.WithMessage("Specified argument was out of the range of valid values." + Environment.NewLine + "Parameter name: similarity");
		}

		[Test]
		public void ValidatePathExist_WithValidPath_ShouldPass()
		{
			var pathToRealFile = Directory.GetCurrentDirectory() + "\\SikuliWrapper.dll";
			Action action = () => new FileImage(pathToRealFile, 0.7);

			action.Should().NotThrow();
		}

		[Test]
		public void ValidatePathExist_WithInvalidValidPath_ShouldThrowException()
		{
			var pathToRealFile = Directory.GetCurrentDirectory() + "\\Batman.dll";
			Action action = () => new FileImage(pathToRealFile, 0.7);

			action.Should()
				.Throw<FileNotFoundException>()
				.WithMessage("Could not find image file specified in pattern: " + pathToRealFile);
		}

		[Test]
		[TestCase(0.3d, "0.3")]
		[TestCase(0.5d, "0.5")]
		[TestCase(0.7d, "0.7")]
		public void ConvertToSikuliScript_ReturnRightSikuliScript(double number, string text)
		{
			var pathToTestPic = Path.GetFullPath(@"..\..\..\Utils\vs.png");
			var actualResult =  new FileImage(Path.GetFullPath(pathToTestPic), number).GeneratePatternString();

			actualResult.Should().Be($@"Pattern(""{pathToTestPic}"").similar({text})");
		}

		[Test]
		public void ToSikuliScript_WithValidPositiveSimilarity_ShouldReturnRightString()
		{
			var pathToRealFile = Directory.GetCurrentDirectory() + "\\SikuliWrapper.dll";
			var pattern = new FileImage(pathToRealFile, 0.7);

			var actualResult = pattern.ToSikuliScript("click", 0.7);

			actualResult.Should()
				.Be($"print \"SIKULI#: YES\" if click({pattern.GeneratePatternString()}, 0.7) else \"SIKULI#: NO\"");
		}
	}
}