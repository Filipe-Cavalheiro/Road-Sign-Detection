using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SS_OpenCV
{
    internal class NumbParam
    {
        // Properties for the object's coordinates
        public int Blue { get; set; }
        public int Green { get; set; }
        public int Red { get; set; }
        public int Area { get; set; }

        // Optional values
        public (int x, int y) Top { get; set; }
        public (int x, int y) Left { get; set; }
        public (int x, int y) Bottom { get; set; }
        public (int x, int y) Right { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public double radiusVariation { get; set; }
        public int CenterX { get; set; }
        public int CenterY { get; set; }
        public int diameter { get; set; }

        public int objectType { get; set; }

        public int[] radius { get; set; }

        // Constructor to initialize the mandatory values
        public NumbParam(int b, int g, int r, int area)
        {
            Blue = b;
            Green = g;
            Red = r;
            Area = area;
            Top = (0, 0);
            Left = (0, 0);
            Bottom = (0, 0);
            Right = (0, 0);
            Width = 0;
            Height = 0;
            CenterX = 0;
            CenterY = 0;
            radius = new int[] { 0, 0, 0, 0, 0, 0, 0, 0 }; // Initialize with 8 values
            diameter = 0;
            radiusVariation = 0.0;
            objectType = -1;
        }
    }
}

