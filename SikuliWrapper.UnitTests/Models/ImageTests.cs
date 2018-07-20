namespace SikuliWrapper.UnitTests.Models
{
	using System.IO;
	using FluentAssertions;
	using NUnit.Framework;
	using SikuliWrapper.Models;
	using SikuliWrapper.Utilities;

	[TestFixture]
    public class ImageTests
    {
	    [Test]
	    public void GenerateImage_WithValidFile_ReturnFilePattern()
	    {
		    var pathToTestPic = Path.GetFullPath(@"..\..\..\Utils\vs.png");
		    var actualResult = ImageFactory.FromFile(pathToTestPic);

			var expectedResult = new FileImage(pathToTestPic, 0.7);
			actualResult.Should().BeEquivalentTo(expectedResult);
	    }

	    [Test]
	    public void GenerateImage_WithValidLocation_ReturnFilePattern()
	    {
		    var actualResult = ImageFactory.Location(500, 500);
			
		    actualResult.Should().BeOfType<Location>();
	    }
    }
}
