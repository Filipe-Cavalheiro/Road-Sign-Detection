using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV.Structure;
using Emgu.CV;

namespace SS_OpenCV
{
    public class Params
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
        public int CenterX { get; set; }
        public int CenterY { get; set; }
        public int objectType { get; set; }

        public void getCoords(Image<Bgr, byte> imgDest, Params obj)
        {
            unsafe
            {
                int width = imgDest.Width;
                int height = imgDest.Height;

                MIplImage m = imgDest.MIplImage;
                byte* dataPtrOrg = (byte*)m.ImageData.ToPointer();     // Pointer to the image
                int nChan = m.NChannels;                            // number of channels = 3
                int padding = m.WidthStep - m.NChannels * m.Width;  // alinhament bytes (padding)
                int widthStep = m.WidthStep;

                int x, y;
                (int x, int y) top = (-1, -1), left = (-1, -1), bottom = (-1, -1), right = (-1, -1);
                byte* dataPtr = dataPtrOrg;

                dataPtr = dataPtrOrg;
                //loop lines first
                for (y = 0; y < height; y++)
                {
                    for (x = 0; x < width; x++)
                    {
                        if (dataPtr[0] == obj.Blue && dataPtr[1] == obj.Green && dataPtr[2] == obj.Red)
                        {
                            top = (x, y);
                            x = width; //force exit of for loops
                            y = height;
                        }
                        dataPtr += nChan;
                    }
                    dataPtr += padding;
                }

                // Loop through columns first
                for (x = 0; x < width; x++)
                {
                    dataPtr = dataPtrOrg + x * nChan;
                    for (y = 0; y < height; y++)
                    {
                        if (dataPtr[0] == obj.Blue && dataPtr[1] == obj.Green && dataPtr[2] == obj.Red)
                        {
                            left = (x, y);
                            x = width; //force exit of for loops
                            y = height;
                        }

                        dataPtr += widthStep;
                    }
                }

                dataPtr = dataPtrOrg + height * (width * nChan);
                for (y = height; y > 0; y--)
                {
                    for (x = 0; x < width; x++)
                    {
                        if (dataPtr[0] == obj.Blue && dataPtr[1] == obj.Green && dataPtr[2] == obj.Red)
                        {
                            bottom = (x, y);
                            x = width; //force exit of for loops
                            y = 0;
                        }
                        dataPtr -= nChan;
                    }
                    dataPtr -= padding;
                }

                // Loop through columns first starting at right
                for (x = width; x > 0; x--)
                {
                    dataPtr = dataPtrOrg + x * nChan;
                    for (y = 0; y < height; y++)
                    {
                        if (dataPtr[0] == obj.Blue && dataPtr[1] == obj.Green && dataPtr[2] == obj.Red)
                        {
                            right = (x, y);
                            x = 0; //force exit of for loops
                            y = height;
                        }

                        dataPtr += width * nChan + padding;
                    }
                }
                if (top.y == -1 || left.x == -1 || right.x == -1 || bottom.y == -1)
                    throw new InvalidOperationException("One or more values are -1, which is not allowed.");

                obj.Top = top;
                obj.Bottom = bottom;
                obj.Right = right;
                obj.Left = left;
                obj.Width = right.x - left.x;
                obj.Height = bottom.y - top.y;
            }
        }
    }
    internal class ObjectParams : Params
    {

    // Optional values
        public double radiusVariation { get; set; } 
        public int CenterX { get; set; } 
        public int CenterY { get; set; }
        public int innerDiameter { get; set; }

        public int[] radius { get; set; }
        public int FilledArea { get; set; }
        public float Circularity { get; set; }
        public float Triangularity { get; set; }
        public List<NumbParam> numbers { get; set; }

        // Constructor to initialize the mandatory values
        public ObjectParams(int b, int g, int r, int area)
        {
            Blue = b;
            Green = g;
            Red = r;
            Area = area;
            Top = (0, 0);
            Left = (0, 0);
            Bottom = (0, 0);
            Right = (0, 0);
            Width = -1; 
            Height = -1;
            CenterX = -1;
            CenterY = -1;
            radius = new int[] { 0, 0, 0, 0, 0, 0, 0, 0 };
            innerDiameter = -1;
            radiusVariation = 0.0;
            objectType = -1;
            numbers = new List<NumbParam>();
            FilledArea = -1;
            Circularity = -1;
            Triangularity = -1;
        }

        public static Dictionary<(byte B, byte G, byte R), ObjectParams> removeSmallAreas(Image<Bgr, byte> imgDest, int area)
        {
            unsafe
            {
                int width = imgDest.Width;
                int height = imgDest.Height;

                MIplImage m = imgDest.MIplImage;
                byte* dataPtr = (byte*)m.ImageData.ToPointer(); // Pointer to the image
                int nChan = m.NChannels; // number of channels = 3
                int padding = m.WidthStep - m.NChannels * m.Width; // alinhament bytes (padding)

                Dictionary<(byte, byte, byte), ObjectParams> objects = new Dictionary<(byte, byte, byte), ObjectParams>();

                int x, y;
                for (y = 0; y < height; y++)
                {
                    for (x = 0; x < width; x++)
                    {
                        if (dataPtr[0] != 0 || dataPtr[1] != 0 || dataPtr[2] != 0)
                        {
                            var colorKey = (dataPtr[0], dataPtr[1], dataPtr[2]);

                            // Update the count in the dictionary
                            if (objects.ContainsKey(colorKey))
                            {
                                objects[colorKey].Area++;
                            }
                            else
                            {
                                objects[colorKey] = new ObjectParams(dataPtr[0], dataPtr[1], dataPtr[2], 1);
                            }
                        }
                        // advance the pointer to the next pixel
                        dataPtr += nChan;
                    }

                    //at the end of the line advance the pointer by the aligment bytes (padding)
                    dataPtr += padding;
                }

                var largestEntries = objects
                    .Where(kvp => kvp.Value.Area < area)
                    .Select(kvp => kvp.Key)
                    .ToList();

                // Remove the keys from the dictionary
                foreach (var key in largestEntries)
                {
                    objects.Remove(key);
                }
                return objects;
            }
        }


        public static void identifyObjects(Dictionary<(byte B, byte G, byte R), ObjectParams> objects)
        {
            int maxTopY;
            int minTopY;
            int maxheight;
            int minheight;

            foreach (var sign_object in objects)
            {
                //cicles
                if ((0.9 < sign_object.Value.Circularity && sign_object.Value.Circularity < 1.2) || (sign_object.Value.radiusVariation < 50 && sign_object.Value.innerDiameter > 20))
                {
                    sign_object.Value.objectType = 13;
                    if (sign_object.Value.numbers.Count != 0) {
                        maxheight = sign_object.Value.numbers.Max(x => x.Height);
                        minheight = sign_object.Value.numbers.Min(x => x.Height);
                        if ((double)((maxheight - minheight) / (double)maxheight) * 100 < 20)
                            sign_object.Value.objectType = 10;
                    }
                }
                //triangles
                else if (0.9 < sign_object.Value.Triangularity && sign_object.Value.Triangularity < 2.5)
                {
                    maxTopY = Math.Max(sign_object.Value.Top.y, Math.Max(sign_object.Value.Left.y, sign_object.Value.Right.y));
                    minTopY = Math.Min(sign_object.Value.Top.y, Math.Min(sign_object.Value.Left.y, sign_object.Value.Right.y));
                    double variance = (double)((maxTopY - minTopY) / (double)maxTopY) * 100;
                    if (variance < 10)
                        sign_object.Value.objectType = 11;
                    else
                        sign_object.Value.objectType = 12;
                }
            }
        }
    }
}
