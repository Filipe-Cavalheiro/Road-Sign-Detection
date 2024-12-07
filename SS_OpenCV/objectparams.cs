using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SS_OpenCV
{
    internal class ObjectParams
    {
        // Properties for the object's coordinates
        public int Blue { get; set; }
        public int Green { get; set; }
        public int Red { get; set; }
        public int Area { get; set; }

    // Optional values
        public int Top { get; set; }
        public int Left { get; set; }
        public int Bottom { get; set; }
        public int Right { get; set; }
        public double radiusVariation { get; set; } 
        public int CenterX { get; set; } 
        public int CenterY { get; set; }

        public int diameter { get; set; }

        // Constructor to initialize the mandatory values
        public ObjectParams(int b, int g, int r, int area)
        {
            Blue = b;
            Green = g;
            Red = r;
            Area = area;
            Top = 0;
            Left = 0;
            Bottom = 0;
            Right = 0;
            CenterX = 0;
            CenterY = 0;
            diameter = 0;
            radiusVariation = 0.0;
        }
    }
}
