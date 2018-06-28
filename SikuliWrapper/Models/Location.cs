namespace SikuliWrapper.Models
{
	using System;

	public class Location
	{
		private readonly Point _point;

		public Location(int x, int y)
			: this(new Point(x, y))
		{
		}

		private Location(Point point)
		{
			_point = point;
		}

		public void Validate()
		{
			if(_point.X < 0 || _point.Y < 0)
			{
				throw new Exception("Cannot target a negative position");
			}
		}

		public string ToSikuliScript()
		{
			return $"Location({_point.X},{_point.Y})";
		}
	}
}
