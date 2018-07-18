namespace SikuliWrapper.UnitTests.Models
{
	using System;
	using FluentAssertions;
	using NUnit.Framework;
	using SikuliWrapper.Models;

	[TestFixture]
    public class LocationTests
    {
	    [Test]
		[TestCase(-1, 1)]
		[TestCase(1, -1)]
	    public void ValidateLocation_WithNegativeNumbers_ShouldThrowException(int x, int y)
	    {
		    var location =  new Location(x, y);

		    location.Invoking(l => l.Validate())
			    .Should().Throw<ArgumentException>()
			    .WithMessage("Cannot target a negative position");
	    }

	    [Test]
	    public void ValidateLocation_WithPositiveNumbers_ShouldPass()
	    {
		    var location =  new Location(1, 1);

		    location.Invoking(l => l.Validate()).Should().NotThrow();
	    }

	    [Test]
	    public void LocationToScript_ShouldReturnRightScript()
	    {
		    var location =  new Location(1, 1);
		    var script = location.ToSikuliScript();

		    script.Should().Be("Location(1,1)");
	    }
    }
}
