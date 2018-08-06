namespace SikuliWrapper.UnitTests.Models
{
	using System;
	using System.IO;
	using FluentAssertions;
	using NUnit.Framework;
	using SikuliWrapper.Models;

	[TestFixture]
	public class OffsetPatternTests
	{
		[Test]
		public void CreateOffsetPattern_WithNullPattern_ShouldThrowException()
		{
			var point = new Point(5, 5);
			Action action = () => new OffsetImage(null, point);

			action.Should()
				.Throw<NullReferenceException>();
		}

		[Test]
		public void CreateOffsetPattern_WithPattern_ShouldPass()
		{
			var pattern = new FileImage(Path.GetFullPath(@"..\..\..\Utils\vs.png"), 0.3);
			var point = new Point(5, 5);
			Action action = () => new OffsetImage(pattern, point);

			action.Should().NotThrow();
		}

		[Test]
		public void OffsetPatternToScript_ShouldReturnRightString()
		{
			var pathToTestPic = Path.GetFullPath(@"..\..\..\Utils\vs.png");
			var pattern = new FileImage(pathToTestPic, 0.3);
			var point = new Point(5, 5);
			var offsetPattern =  new OffsetImage(pattern, point);

			var actualResult = offsetPattern.ToSikuliScript("click", 0);

			actualResult.Should()
				.Be(
					$@"print ""SIKULI#: YES"" if click(Pattern({"\"" + pathToTestPic + "\""}).similar(0.3).targetOffset(5, 5)) else ""SIKULI#: NO""");
		}
	}
}
