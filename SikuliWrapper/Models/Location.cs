namespace SikuliWrapper.Models
{
	using System;

	public class Location
	{
		private readonly Point _pointtttt;

		public Location(int x, int y)
			: this(new Point(x, y))
		{
		}

		private Location(Point point)
		{
			_pointtttt = point;
		}

		public void Validate()
		{
			if(_pointtttt.X < 0 || _pointtttt.Y < 0)
			{
				throw new ArgumentException("Cannot target a negative position");
			}
		}

		public string ToSikuliScript()
		{
			return $"Location({_pointtttt.X},{_pointtttt.Y})";
		}
	}
}
