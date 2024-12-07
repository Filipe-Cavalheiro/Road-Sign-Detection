using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV.Structure;
using Emgu.CV;

namespace SS_OpenCV
{
    internal class NumbParam : Params
    {
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
            diameter = 0;
            objectType = -1;
        }

        public static Dictionary<(byte B, byte G, byte R), NumbParam> removeSmallAreas(Image<Bgr, byte> imgDest, int area)
        {
            unsafe
            {
                int width = imgDest.Width;
                int height = imgDest.Height;

                MIplImage m = imgDest.MIplImage;
                byte* dataPtr = (byte*)m.ImageData.ToPointer(); // Pointer to the image
                int nChan = m.NChannels; // number of channels = 3
                int padding = m.WidthStep - m.NChannels * m.Width; // alinhament bytes (padding)

                Dictionary<(byte, byte, byte), NumbParam> objects = new Dictionary<(byte, byte, byte), NumbParam>();

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
                                objects[colorKey] = new NumbParam(dataPtr[0], dataPtr[1], dataPtr[2], 1);
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
    }
}

