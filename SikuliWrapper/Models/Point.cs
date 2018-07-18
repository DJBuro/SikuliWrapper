﻿namespace SikuliWrapper.Models
{
    public struct Point
    {
        public Point(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

	    public int X { get; private set; }

	    public int Y { get; private set; }
    }
}
