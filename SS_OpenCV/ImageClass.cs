﻿using System;
using System.Collections.Generic;
using System.Text;
using Emgu.CV.Structure;
using Emgu.CV;
using System.Drawing;
using System.Management.Instrumentation;
using System.Windows.Forms;
using Emgu.CV.ImgHash;
using System.Linq;
using Emgu.CV.XFeatures2D;
using System.Diagnostics.Eventing.Reader;
using System.Diagnostics.Contracts;
using Emgu.CV.XImgproc;
using System.Runtime.InteropServices.ComTypes;
using Emgu.CV.ML;
using ResultsDLL;
using Emgu.CV.CvEnum;
using System.Runtime.ExceptionServices;
using System.Collections.Concurrent;
using System.Reflection.Emit;
using Emgu.CV.Flann;
using System.Diagnostics.Tracing;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar;
using System.Security.Cryptography;
using System.Reflection;
using System.Runtime.Serialization.Formatters;

namespace SS_OpenCV
{
    class ImageClass
    {
        /// <summary>
        /// Image Negative using EmguCV library
        /// Slower method
        /// </summary>
        /// <param name="img">Image</param>
        public static void Negative(Image<Bgr, byte> img)
        {
            unsafe
            {
                // direct access to the image memory(sequencial)
                // direcion top left -> bottom right

                MIplImage m = img.MIplImage;
                byte* dataPtr = (byte*)m.ImageData.ToPointer(); // Pointer to the image
                
                int width = img.Width;
                int height = img.Height;
                int nChan = m.NChannels; // number of channels = 3
                int padding = m.WidthStep - m.NChannels * m.Width; // alinhament bytes (padding)
                int x, y;

                if (nChan == 3) // image in RGB
                {
                    for (y = 0; y < height; y++)
                    {
                        for (x = 0; x < width; x++)
                        {

                            // convert to negative
                            dataPtr[0] = (byte)(255 - dataPtr[0]);
                            dataPtr[1] = (byte)(255 - dataPtr[1]);
                            dataPtr[2] = (byte)(255 - dataPtr[2]);

                            // advance the pointer to the next pixel
                            dataPtr += nChan;
                        }

                        //at the end of the line advance the pointer by the aligment bytes (padding)
                        dataPtr += padding;
                    }
                }
            }
        }

        /// <summary>
        /// Convert to gray
        /// Direct access to memory - faster method
        /// </summary>
        /// <param name="img">image</param>
        public static void ConvertToGray(Image<Bgr, byte> img)
        {
            unsafe
            {
                // direct access to the image memory(sequencial)
                // direcion top left -> bottom right

                MIplImage m = img.MIplImage;
                byte* dataPtr = (byte*)m.ImageData.ToPointer(); // Pointer to the image
                byte gray;

                int width = img.Width;
                int height = img.Height;
                int nChan = m.NChannels; // number of channels = 3
                int padding = m.WidthStep - m.NChannels * m.Width; // alinhament bytes (padding)
                int x, y;

                if (nChan == 3) // image in RGB
                {
                    for (y = 0; y < height; y++)
                    {
                        for (x = 0; x < width; x++)
                        {

                            // convert to gray
                            gray = (byte)Math.Round(((int)dataPtr[0] + dataPtr[1] + dataPtr[2]) / 3.0);

                            // store in the image
                            dataPtr[0] = gray;
                            dataPtr[1] = gray;
                            dataPtr[2] = gray;

                            // advance the pointer to the next pixel
                            dataPtr += nChan;
                        }

                        //at the end of the line advance the pointer by the aligment bytes (padding)
                        dataPtr += padding;
                    }
                }
            }
        }

        /// <summary>
        /// Convert to red
        /// </summary>
        /// <param name="img">image</param>
        public static void RedChannel(Image<Bgr, byte> img)
        {
            unsafe
            {
                // direct access to the image memory(sequencial)
                // direcion top left -> bottom right

                MIplImage m = img.MIplImage;
                byte* dataPtr = (byte*)m.ImageData.ToPointer(); // Pointer to the image

                int width = img.Width;
                int height = img.Height;
                int nChan = m.NChannels; // number of channels = 3
                int padding = m.WidthStep - m.NChannels * m.Width; // alinhament bytes (padding)
                int x, y;

                if (nChan == 3) // image in RGB
                {
                    for (y = 0; y < height; y++)
                    {
                        for (x = 0; x < width; x++)
                        {
                            // store in the image
                            dataPtr[0] = dataPtr[2];
                            dataPtr[1] = dataPtr[2];

                            // advance the pointer to the next pixel
                            dataPtr += nChan;
                        }

                        //at the end of the line advance the pointer by the aligment bytes (padding)
                        dataPtr += padding;
                    }
                }
            }
        }

        /// <summary>
        /// Convert to blue
        /// Direct access to memory - faster method
        /// </summary>
        /// <param name="img">image</param>
        public static void BlueChannel(Image<Bgr, byte> img)
        {
            unsafe
            {
                MIplImage m = img.MIplImage;
                byte* dataPtr = (byte*)m.ImageData.ToPointer(); // Pointer to the image

                int x, y, height, width, nChan, padding;

                height = img.Height;
                width = img.Width;

                nChan = m.NChannels; // number of channels - 3
                padding = m.WidthStep - m.NChannels * m.Width; // alinhament bytes (padding)

                if (nChan != 3) return;

                for (y = 0; y < height; ++y)
                {
                    for (x = 0; x < width; ++x)
                    {
                        dataPtr[2] = dataPtr[1] = dataPtr[0];

                        dataPtr += nChan;
                    }
                    dataPtr += padding;
                }
            }
        }

        /// <summary>
        /// Convert to green
        /// Direct access to memory - faster method
        /// </summary>
        /// <param name="img">image</param>
        public static void GreenChannel(Image<Bgr, byte> img)
        {
            unsafe
            {
                MIplImage m = img.MIplImage;
                byte* dataPtr = (byte*)m.ImageData.ToPointer(); // Pointer to the image

                int x, y, height, width, nChan, padding;

                height = img.Height;
                width = img.Width;

                nChan = m.NChannels; // number of channels - 3
                padding = m.WidthStep - m.NChannels * m.Width; // alinhament bytes (padding)

                if (nChan != 3) return;

                for (y = 0; y < height; ++y)
                {
                    for (x = 0; x < width; ++x)
                    {
                        dataPtr[2] = dataPtr[0] = dataPtr[1];

                        dataPtr += nChan;
                    }
                    dataPtr += padding;
                }
            }
        }

        /// <summary>
        /// Convert to gray
        /// Direct access to memory - faster method
        /// </summary>
        /// <param name="img">image</param>
        public static void BrightContrast(Image<Bgr, byte> img, int bright, double contrast)
        {
            unsafe
            {
                // direct access to the image memory(sequencial)
                // direcion top left -> bottom right

                MIplImage m = img.MIplImage;
                byte* dataPtr = (byte*)m.ImageData.ToPointer(); // Pointer to the image

                int width = img.Width;
                int height = img.Height;
                int nChan = m.NChannels; // number of channels = 3
                int padding = m.WidthStep - m.NChannels * m.Width; // alinhament bytes (padding)
                int x, y;
                int blue, green, red;
                if (nChan == 3) // image in RGB
                {
                    for (y = 0; y < height; y++)
                    {
                        for (x = 0; x < width; x++)
                        {
                            blue = (int)Math.Round(contrast * dataPtr[0] + bright);
                            if (blue > 255)
                                blue = 255;
                            else if (blue < 0)
                                blue = 0;
                            dataPtr[0] = (byte)blue;

                            green = (int)Math.Round(contrast * dataPtr[1] + bright);
                            if (green > 255)
                                green = 255;
                            else if (green < 0)
                                green = 0;
                            dataPtr[1] = (byte)green;

                            red = (int)Math.Round(contrast * dataPtr[2] + bright);
                            if (red > 255)
                                red = 255;
                            else if (red < 0)
                                red = 0;
                            dataPtr[2] = (byte)red;

                            // advance the pointer to the next pixel
                            dataPtr += nChan;
                        }

                        //at the end of the line advance the pointer by the aligment bytes (padding)
                        dataPtr += padding;
                    }
                }
            }
        }

        /// <summary>
        /// Convert to gray
        /// Direct access to memory - faster method
        /// </summary>
        /// <param name="img">image</param>
        public static void Rotation(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy, float radAngle)
        {
            unsafe
            {
                // direct access to the image memory(sequencial)
                // direcion top left -> bottom right

                unsafe
                {
                    int x, y, height, width, nChan, padding, widthStep, blue, green, red, ox, oy;
                    float cosa, sina;
                    double cx, cy;

                    cosa = (float)Math.Cos(radAngle);
                    sina = (float)Math.Sin(radAngle);

                    height = img.Height;
                    width = img.Width;

                    cx = width / 2.0;
                    cy = height / 2.0;

                    MIplImage mo = imgCopy.MIplImage;
                    MIplImage md = img.MIplImage;

                    byte* dataPtrd = (byte*)md.ImageData.ToPointer();
                    byte* dataPtro = (byte*)mo.ImageData.ToPointer();
                    byte* dataPtrAux;

                    nChan = md.NChannels;
                    widthStep = md.WidthStep;
                    padding = md.WidthStep - md.NChannels * md.Width;

                    for (y = 0; y < height; y++)
                    {
                        for (x = 0; x < width; x++)
                        {
                            ox = (int)Math.Round((x - cx) * cosa - (cy - y) * sina + cx);
                            oy = (int)Math.Round(cy - (x - cx) * sina - (cy - y) * cosa);

                            if ((ox >= 0 && ox < width) && (oy >= 0 && oy < height))
                            {
                                dataPtrAux = dataPtro + oy * widthStep + ox * nChan;

                                blue = (byte)(dataPtrAux)[0];
                                green = (byte)(dataPtrAux)[1];
                                red = (byte)(dataPtrAux)[2];

                            }
                            else { blue = green = red = 0; }

                            dataPtrd[0] = (byte)blue;
                            dataPtrd[1] = (byte)green;
                            dataPtrd[2] = (byte)red;

                            dataPtrd += nChan;
                        }
                        dataPtrd += padding;
                    }
                }
            }
        }

        public static void Translation(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy, int dx, int dy)
        {
            unsafe
            {
                // direct access to the image memory(sequencial)
                // direcion top left -> bottom right

                MIplImage m = img.MIplImage;
                byte* dataPtr = (byte*)m.ImageData.ToPointer(); // Pointer to the image

                MIplImage Originm = imgCopy.MIplImage;
                byte* OrigindataPtr = (byte*)Originm.ImageData.ToPointer(); // Pointer to the image

                int width = img.Width;
                int height = img.Height;
                int nChan = m.NChannels; // number of channels = 3
                int padding = m.WidthStep - m.NChannels * m.Width; // alinhament bytes (padding)
                byte* OrigindataPtr_axu;
                int x, y;
                int xOrigin, yOrigin;
                //double radAngle = (Math.PI / 180) * angle;
                if (nChan == 3) // image in RGB
                {
                    for (y = 0; y < height; y++)
                    {
                        for (x = 0; x < width; x++)
                        {
                            xOrigin = x - dx;
                            yOrigin = y - dy;
                            if ((xOrigin < 0 || yOrigin < 0) || (xOrigin >= width || yOrigin >= height))
                            {
                                dataPtr[0] = 0;
                                dataPtr[1] = 0;
                                dataPtr[2] = 0;
                                dataPtr += nChan;
                                continue;
                            }

                            OrigindataPtr_axu = OrigindataPtr + (xOrigin * 3 + yOrigin * (width * 3 + padding));
                            dataPtr[0] = OrigindataPtr_axu[0];
                            dataPtr[1] = OrigindataPtr_axu[1];
                            dataPtr[2] = OrigindataPtr_axu[2];

                            // advance the pointer to the next pixel
                            dataPtr += nChan;
                        }

                        //at the end of the line advance the pointer by the aligment bytes (padding)
                        dataPtr += padding;
                    }
                }
            }
        }
        public static void Scale(Image<Bgr, byte> imgDestino, Image<Bgr, byte> imgOrigem, float scaleFactor)
        {
            unsafe
            {
                // direct access to the image memory(sequencial)
                // direcion top left -> bottom right

                MIplImage m = imgDestino.MIplImage;
                byte* dataPtr = (byte*)m.ImageData.ToPointer(); // Pointer to the image

                MIplImage Originm = imgOrigem.MIplImage;
                byte* OrigindataPtr = (byte*)Originm.ImageData.ToPointer(); // Pointer to the image

                int width = imgDestino.Width;
                int height = imgDestino.Height;
                int nChan = m.NChannels; // number of channels = 3
                int padding = m.WidthStep - m.NChannels * m.Width; // alinhament bytes (padding)
                byte* OrigindataPtr_axu;
                int x, y;
                int xOrigin, yOrigin;
                //double radAngle = (Math.PI / 180) * angle;
                if (nChan == 3) // image in RGB
                {
                    for (y = 0; y < height; y++)
                    {
                        for (x = 0; x < width; x++)
                        {
                            xOrigin = (int)(x / scaleFactor);
                            yOrigin = (int)(y / scaleFactor);
                            if ((xOrigin < 0 || yOrigin < 0) || (xOrigin >= width || yOrigin >= height))
                            {
                                dataPtr[0] = 0;
                                dataPtr[1] = 0;
                                dataPtr[2] = 0;
                                dataPtr += nChan;
                                continue;
                            }

                            OrigindataPtr_axu = OrigindataPtr + (xOrigin * 3 + yOrigin * (width * 3 + padding));
                            dataPtr[0] = OrigindataPtr_axu[0];
                            dataPtr[1] = OrigindataPtr_axu[1];
                            dataPtr[2] = OrigindataPtr_axu[2];

                            // advance the pointer to the next pixel
                            dataPtr += nChan;
                        }

                        //at the end of the line advance the pointer by the aligment bytes (padding)
                        dataPtr += padding;
                    }
                }
            }
        }

        public static void Scale_point_xy(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy, float scaleFactor, int centerX, int centerY)
        {
            unsafe
            {
                // direct access to the image memory(sequencial)
                // direcion top left -> bottom right

                MIplImage m = img.MIplImage;
                byte* dataPtr = (byte*)m.ImageData.ToPointer(); // Pointer to the image

                MIplImage Originm = imgCopy.MIplImage;
                byte* OrigindataPtr = (byte*)Originm.ImageData.ToPointer(); // Pointer to the image

                int width = img.Width;
                int height = img.Height;
                int nChan = m.NChannels; // number of channels = 3
                int padding = m.WidthStep - m.NChannels * m.Width; // alinhament bytes (padding)
                byte* OrigindataPtr_axu;
                int x, y;
                double Xoffset = -(width / 2) / scaleFactor + centerX;
                double Yoffset = -(height / 2) / scaleFactor + centerY;
                int xOrigin, yOrigin;
                if (nChan == 3) // image in RGB
                {
                    for (y = 0; y < height; y++)
                    {
                        for (x = 0; x < width; x++)
                        {
                            xOrigin = (int)Math.Round(x / scaleFactor + Xoffset);
                            yOrigin = (int)Math.Round(y / scaleFactor + Yoffset);
                            if ((xOrigin < 0 || yOrigin < 0) || (xOrigin >= width || yOrigin >= height))
                            {
                                dataPtr[0] = 0;
                                dataPtr[1] = 0;
                                dataPtr[2] = 0;
                                dataPtr += nChan;
                                continue;
                            }

                            OrigindataPtr_axu = OrigindataPtr + (xOrigin * 3 + yOrigin * (width * 3 + padding));
                            dataPtr[0] = OrigindataPtr_axu[0];
                            dataPtr[1] = OrigindataPtr_axu[1];
                            dataPtr[2] = OrigindataPtr_axu[2];

                            // advance the pointer to the next pixel
                            dataPtr += nChan;
                        }

                        //at the end of the line advance the pointer by the aligment bytes (padding)
                        dataPtr += padding;
                    }
                }
            }
        }
        public static void Mean(Image<Bgr, byte> imgDest, Image<Bgr, byte> imgOrig)
        {
            unsafe
            {
                int x, y, i, j, height, width, nChan, padding, widthStep, filterSize, filterPadding, sumGreen, sumBlue, sumRed;
                float filterArea;

                filterSize = 3;
                filterArea = filterSize * filterSize;
                filterPadding = filterSize / 2;

                height = imgDest.Height;
                width = imgDest.Width;

                MIplImage mo = imgOrig.MIplImage;
                MIplImage md = imgDest.MIplImage;

                byte* dataPtrd = (byte*)md.ImageData.ToPointer();
                byte* dataPtro = (byte*)mo.ImageData.ToPointer();
                byte* dataPtrAux;

                nChan = md.NChannels;
                widthStep = md.WidthStep;
                padding = md.WidthStep - md.NChannels * md.Width;

                //Canto superior esquerdo
                for (y = 0; y < filterPadding; ++y)
                {
                    for (x = 0; x < filterPadding; ++x)
                    {
                        /**
                        *  0**
                        *  ***
                        *  ***
                        */
                        dataPtrAux = dataPtro;
                        sumBlue = dataPtrAux[0] * (filterPadding + 1 - x) * (filterPadding + 1 - y);
                        sumGreen = dataPtrAux[1] * (filterPadding + 1 - x) * (filterPadding + 1 - y);
                        sumRed = dataPtrAux[2] * (filterPadding + 1 - x) * (filterPadding + 1 - y);

                        dataPtrAux += nChan;

                        /**
                        *  *00
                        *  ***
                        *  ***
                        */

                        for (i = 0; i < filterPadding; ++i)
                        {
                            sumBlue += dataPtrAux[0] * (filterPadding + 1 - y);
                            sumGreen += dataPtrAux[1] * (filterPadding + 1 - y);
                            sumRed += dataPtrAux[2] * (filterPadding + 1 - y);

                            dataPtrAux += nChan;
                        }

                        dataPtrAux -= nChan * (filterPadding + 1) - widthStep;

                        /**
                        *  ***
                        *  0**
                        *  0**
                        */
                        for (i = 0; i < filterPadding; ++i)
                        {
                            sumBlue += dataPtrAux[0] * (filterPadding + 1 - x);
                            sumGreen += dataPtrAux[1] * (filterPadding + 1 - x);
                            sumRed += dataPtrAux[2] * (filterPadding + 1 - x);

                            dataPtrAux += widthStep;
                        }

                        dataPtrAux -= filterPadding * widthStep - nChan;

                        /**
                        *  ***
                        *  *00
                        *  *00
                        */
                        for (i = 0; i < filterPadding; ++i)
                        {
                            for (j = 0; j < filterPadding; ++j)
                            {
                                sumBlue += dataPtrAux[0];
                                sumGreen += dataPtrAux[1];
                                sumRed += dataPtrAux[2];

                                dataPtrAux += nChan;
                            }
                            dataPtrAux -= nChan * filterPadding;
                            dataPtrAux += widthStep;
                        }
                        dataPtrd[0] = (byte)Math.Round(sumBlue / filterArea);
                        dataPtrd[1] = (byte)Math.Round(sumGreen / filterArea);
                        dataPtrd[2] = (byte)Math.Round(sumRed / filterArea);

                        dataPtrd += nChan;
                    }
                    dataPtrd -= nChan * filterPadding;
                    dataPtrd += widthStep;
                }

                //Canto superior direito
                dataPtrd = (byte*)md.ImageData.ToPointer() + (width - 1) * nChan;

                for (y = 0; y < filterPadding; ++y)
                {
                    for (x = 0; x < filterPadding; ++x)
                    {
                        dataPtrAux = dataPtro + (width - 1) * nChan;
                        sumBlue = dataPtrAux[0] * (filterPadding + 1 - x) * (filterPadding + 1 - y);
                        sumGreen = dataPtrAux[1] * (filterPadding + 1 - x) * (filterPadding + 1 - y);
                        sumRed = dataPtrAux[2] * (filterPadding + 1 - x) * (filterPadding + 1 - y);

                        dataPtrAux -= nChan;

                        for (i = 0; i < filterPadding; ++i)
                        {
                            sumBlue += dataPtrAux[0] * (filterPadding + 1 - y);
                            sumGreen += dataPtrAux[1] * (filterPadding + 1 - y);
                            sumRed += dataPtrAux[2] * (filterPadding + 1 - y);

                            dataPtrAux -= nChan;
                        }

                        dataPtrAux += nChan * (filterPadding + 1) + widthStep;

                        for (i = 0; i < filterPadding; ++i)
                        {
                            sumBlue += dataPtrAux[0] * (filterPadding + 1 - x);
                            sumGreen += dataPtrAux[1] * (filterPadding + 1 - x);
                            sumRed += dataPtrAux[2] * (filterPadding + 1 - x);

                            dataPtrAux += widthStep;
                        }

                        dataPtrAux -= filterPadding * widthStep + nChan;

                        for (i = 0; i < filterPadding; ++i)
                        {
                            for (j = 0; j < filterPadding; ++j)
                            {
                                sumBlue += dataPtrAux[0];
                                sumGreen += dataPtrAux[1];
                                sumRed += dataPtrAux[2];

                                dataPtrAux -= nChan;
                            }
                            dataPtrAux += nChan * filterPadding;
                            dataPtrAux += widthStep;
                        }
                        dataPtrd[0] = (byte)Math.Round(sumBlue / filterArea);
                        dataPtrd[1] = (byte)Math.Round(sumGreen / filterArea);
                        dataPtrd[2] = (byte)Math.Round(sumRed / filterArea);

                        dataPtrd -= nChan;
                    }
                    dataPtrd += nChan * filterPadding;
                    dataPtrd += widthStep;
                }

                //Canto inferior esquerdo
                dataPtrd = (byte*)md.ImageData.ToPointer() + (height - 1) * widthStep;

                for (y = 0; y < filterPadding; ++y)
                {
                    for (x = 0; x < filterPadding; ++x)
                    {
                        dataPtrAux = dataPtro + (height - 1) * widthStep;

                        sumBlue = dataPtrAux[0] * (filterPadding + 1 - x) * (filterPadding + 1 - y);
                        sumGreen = dataPtrAux[1] * (filterPadding + 1 - x) * (filterPadding + 1 - y);
                        sumRed = dataPtrAux[2] * (filterPadding + 1 - x) * (filterPadding + 1 - y);

                        dataPtrAux += nChan;

                        for (i = 0; i < filterPadding; ++i)
                        {
                            sumBlue += dataPtrAux[0] * (filterPadding + 1 - y);
                            sumGreen += dataPtrAux[1] * (filterPadding + 1 - y);
                            sumRed += dataPtrAux[2] * (filterPadding + 1 - y);

                            dataPtrAux += nChan;
                        }

                        dataPtrAux -= nChan * (filterPadding + 1) + widthStep;

                        for (i = 0; i < filterPadding; ++i)
                        {
                            sumBlue += dataPtrAux[0] * (filterPadding + 1 - x);
                            sumGreen += dataPtrAux[1] * (filterPadding + 1 - x);
                            sumRed += dataPtrAux[2] * (filterPadding + 1 - x);

                            dataPtrAux -= widthStep;
                        }

                        dataPtrAux += filterPadding * widthStep + nChan;

                        for (i = 0; i < filterPadding; ++i)
                        {
                            for (j = 0; j < filterPadding; ++j)
                            {
                                sumBlue += dataPtrAux[0];
                                sumGreen += dataPtrAux[1];
                                sumRed += dataPtrAux[2];

                                dataPtrAux += nChan;
                            }
                            dataPtrAux -= nChan * filterPadding;
                            dataPtrAux -= widthStep;
                        }
                        dataPtrd[0] = (byte)Math.Round(sumBlue / filterArea);
                        dataPtrd[1] = (byte)Math.Round(sumGreen / filterArea);
                        dataPtrd[2] = (byte)Math.Round(sumRed / filterArea);

                        dataPtrd += nChan;
                    }
                    dataPtrd -= nChan * filterPadding;
                    dataPtrd -= widthStep;
                }

                //Canto inferior direito
                dataPtrd = (byte*)md.ImageData.ToPointer() + (height - 1) * widthStep + (width - 1) * nChan;

                for (y = 0; y < filterPadding; ++y)
                {
                    for (x = 0; x < filterPadding; ++x)
                    {
                        dataPtrAux = dataPtro + (height - 1) * widthStep + (width - 1) * nChan;
                        sumBlue = dataPtrAux[0] * (filterPadding + 1 - x) * (filterPadding + 1 - y);
                        sumGreen = dataPtrAux[1] * (filterPadding + 1 - x) * (filterPadding + 1 - y);
                        sumRed = dataPtrAux[2] * (filterPadding + 1 - x) * (filterPadding + 1 - y);

                        dataPtrAux -= nChan;

                        for (i = 0; i < filterPadding; ++i)
                        {
                            sumBlue += dataPtrAux[0] * (filterPadding + 1 - y);
                            sumGreen += dataPtrAux[1] * (filterPadding + 1 - y);
                            sumRed += dataPtrAux[2] * (filterPadding + 1 - y);

                            dataPtrAux -= nChan;
                        }

                        dataPtrAux += nChan * (filterPadding + 1) - widthStep;

                        for (i = 0; i < filterPadding; ++i)
                        {
                            sumBlue += dataPtrAux[0] * (filterPadding + 1 - x);
                            sumGreen += dataPtrAux[1] * (filterPadding + 1 - x);
                            sumRed += dataPtrAux[2] * (filterPadding + 1 - x);

                            dataPtrAux -= widthStep;
                        }

                        dataPtrAux += filterPadding * widthStep - nChan;

                        for (i = 0; i < filterPadding; ++i)
                        {
                            for (j = 0; j < filterPadding; ++j)
                            {
                                sumBlue += dataPtrAux[0];
                                sumGreen += dataPtrAux[1];
                                sumRed += dataPtrAux[2];

                                dataPtrAux -= nChan;
                            }
                            dataPtrAux += nChan * filterPadding;
                            dataPtrAux -= widthStep;
                        }
                        dataPtrd[0] = (byte)Math.Round(sumBlue / filterArea);
                        dataPtrd[1] = (byte)Math.Round(sumGreen / filterArea);
                        dataPtrd[2] = (byte)Math.Round(sumRed / filterArea);

                        dataPtrd -= nChan;
                    }
                    dataPtrd += nChan * filterPadding;
                    dataPtrd -= widthStep;
                }

                //borda superior
                dataPtrd = (byte*)md.ImageData.ToPointer() + filterPadding * nChan;

                for (y = 0; y < filterPadding; ++y)
                {
                    for (x = filterPadding; x < width - filterPadding; ++x)
                    {
                        dataPtrAux = dataPtro + (x - filterPadding) * nChan;
                        sumBlue = sumGreen = sumRed = 0;

                        for (i = 0; i < filterSize; ++i)
                        {
                            sumBlue += dataPtrAux[0] * (filterPadding + 1 - y);
                            sumGreen += dataPtrAux[1] * (filterPadding + 1 - y);
                            sumRed += dataPtrAux[2] * (filterPadding + 1 - y);

                            dataPtrAux += nChan;
                        }

                        dataPtrAux -= nChan * filterSize;
                        dataPtrAux += widthStep;

                        for (i = 0; i < (filterSize - (filterPadding + 1) - y); ++i)
                        {
                            for (j = 0; j < filterSize; ++j)
                            {
                                sumBlue += dataPtrAux[0];
                                sumGreen += dataPtrAux[1];
                                sumRed += dataPtrAux[2];

                                dataPtrAux += nChan;
                            }

                            dataPtrAux -= nChan * filterSize;
                            dataPtrAux += widthStep;
                        }

                        dataPtrd[0] = (byte)Math.Round(sumBlue / filterArea);
                        dataPtrd[1] = (byte)Math.Round(sumGreen / filterArea);
                        dataPtrd[2] = (byte)Math.Round(sumRed / filterArea);

                        dataPtrd += nChan;
                    }
                    dataPtrd += nChan * 2 * filterPadding;
                    dataPtrd += padding;
                }


                //borda esquerda
                dataPtrd = (byte*)md.ImageData.ToPointer() + filterPadding * widthStep;

                for (x = 0; x < filterPadding; ++x)
                {
                    for (y = filterPadding; y < height - filterPadding; ++y)
                    {
                        dataPtrAux = dataPtro + (y - filterPadding) * widthStep;
                        sumBlue = sumGreen = sumRed = 0;

                        for (i = 0; i < filterSize; ++i)
                        {
                            sumBlue += dataPtrAux[0] * (filterPadding + 1 - x);
                            sumGreen += dataPtrAux[1] * (filterPadding + 1 - x);
                            sumRed += dataPtrAux[2] * (filterPadding + 1 - x);

                            dataPtrAux += widthStep;
                        }

                        dataPtrAux -= widthStep * filterSize;
                        dataPtrAux += nChan;

                        for (i = 0; i < (filterSize - (filterPadding + 1) - x); ++i)
                        {
                            for (j = 0; j < filterSize; ++j)
                            {
                                sumBlue += dataPtrAux[0];
                                sumGreen += dataPtrAux[1];
                                sumRed += dataPtrAux[2];

                                dataPtrAux += widthStep;
                            }

                            dataPtrAux -= widthStep * filterSize;
                            dataPtrAux += nChan;
                        }

                        dataPtrd[0] = (byte)Math.Round(sumBlue / filterArea);
                        dataPtrd[1] = (byte)Math.Round(sumGreen / filterArea);
                        dataPtrd[2] = (byte)Math.Round(sumRed / filterArea);

                        dataPtrd += widthStep;
                    }
                    dataPtrd -= (height - 1 - filterPadding) * widthStep;
                    dataPtrd += nChan;
                }

                //borda inferior
                dataPtrd = (byte*)md.ImageData.ToPointer() + (height - filterPadding) * widthStep + nChan * filterPadding;

                for (y = height - filterPadding; y < height; ++y)
                {
                    for (x = filterPadding; x < width - filterPadding; ++x)
                    {
                        dataPtrAux = dataPtro + (y - filterPadding) * widthStep + (x - filterPadding) * nChan;
                        sumBlue = sumGreen = sumRed = 0;

                        for (i = 0; i < (filterSize - (filterPadding + 1)); ++i)
                        {
                            for (j = 0; j < filterSize; ++j)
                            {
                                sumBlue += dataPtrAux[0];
                                sumGreen += dataPtrAux[1];
                                sumRed += dataPtrAux[2];

                                dataPtrAux += nChan;
                            }

                            dataPtrAux -= nChan * filterSize;
                            dataPtrAux += widthStep;
                        }

                        for (i = 0; i < filterSize; ++i)
                        {
                            sumBlue += dataPtrAux[0] * (filterPadding + 1 - (height - 1 - y));
                            sumGreen += dataPtrAux[1] * (filterPadding + 1 - (height - 1 - y));
                            sumRed += dataPtrAux[2] * (filterPadding + 1 - (height - 1 - y));

                            dataPtrAux += nChan;
                        }


                        dataPtrd[0] = (byte)Math.Round(sumBlue / filterArea);
                        dataPtrd[1] = (byte)Math.Round(sumGreen / filterArea);
                        dataPtrd[2] = (byte)Math.Round(sumRed / filterArea);

                        dataPtrd += nChan;
                    }
                    dataPtrd += nChan * 2 * filterPadding;
                    dataPtrd += padding;
                }

                //borda direita
                dataPtrd = (byte*)md.ImageData.ToPointer() + (width - filterPadding) * nChan + widthStep * filterPadding;

                for (x = width - filterPadding; x < width; ++x)
                {
                    for (y = filterPadding; y < height - filterPadding; ++y)
                    {
                        dataPtrAux = dataPtro + (y - filterPadding) * widthStep + (x - filterPadding) * nChan;
                        sumBlue = sumGreen = sumRed = 0;

                        for (i = 0; i < (filterSize - (filterPadding + 1)); ++i)
                        {
                            for (j = 0; j < filterSize; ++j)
                            {
                                sumBlue += dataPtrAux[0];
                                sumGreen += dataPtrAux[1];
                                sumRed += dataPtrAux[2];

                                dataPtrAux += widthStep;
                            }

                            dataPtrAux -= widthStep * filterSize;
                            dataPtrAux += nChan;
                        }

                        for (i = 0; i < filterSize; ++i)
                        {
                            sumBlue += dataPtrAux[0] * (filterPadding + 1 - (width - 1 - x));
                            sumGreen += dataPtrAux[1] * (filterPadding + 1 - (width - 1 - x));
                            sumRed += dataPtrAux[2] * (filterPadding + 1 - (width - 1 - x));

                            dataPtrAux += widthStep;
                        }


                        dataPtrd[0] = (byte)Math.Round(sumBlue / filterArea);
                        dataPtrd[1] = (byte)Math.Round(sumGreen / filterArea);
                        dataPtrd[2] = (byte)Math.Round(sumRed / filterArea);

                        dataPtrd += widthStep;
                    }
                    dataPtrd -= (height - 1) * widthStep;
                    dataPtrd += nChan;
                }

                //centro
                dataPtrd = (byte*)md.ImageData.ToPointer() + filterPadding * nChan + filterPadding * widthStep;

                for (y = filterPadding; y < height - filterPadding; ++y)
                {
                    for (x = filterPadding; x < width - filterPadding; ++x)
                    {
                        dataPtrAux = dataPtro + (y - filterPadding) * widthStep + (x - filterPadding) * nChan;

                        sumGreen = sumBlue = sumRed = 0;

                        for (i = 0; i < filterSize; ++i)
                        {
                            for (j = 0; j < filterSize; ++j)
                            {
                                sumBlue += dataPtrAux[0];
                                sumGreen += dataPtrAux[1];
                                sumRed += dataPtrAux[2];

                                dataPtrAux += nChan;
                            }
                            dataPtrAux -= nChan * filterSize;
                            dataPtrAux += widthStep;
                        }

                        dataPtrd[0] = (byte)Math.Round(sumBlue / filterArea);
                        dataPtrd[1] = (byte)Math.Round(sumGreen / filterArea);
                        dataPtrd[2] = (byte)Math.Round(sumRed / filterArea);

                        dataPtrd += nChan;
                    }
                    dataPtrd += nChan * 2 * filterPadding;
                    dataPtrd += padding;
                }
            }
        }
        public static void NonUniform(Image<Bgr, byte> imgDest, Image<Bgr, byte> imgOrig, float[,] matrix, float matrixWeight, float offset)
        {
            unsafe
            {
                int x, y, i, j, height, width, nChan, padding, widthStepAux, filterSize, filterPadding;
                float sumGreen, sumBlue, sumRed;
                float tmp;

                filterSize = 3;
                filterPadding = filterSize / 2;

                height = imgDest.Height;
                width = imgDest.Width;

                Image<Bgr, byte> imgAux = new Image<Bgr, byte>(width + filterPadding * 2, height + filterPadding * 2);
                CvInvoke.CopyMakeBorder(imgOrig, imgAux, filterPadding, filterPadding, filterPadding, filterPadding, Emgu.CV.CvEnum.BorderType.Replicate);

                MIplImage mo = imgAux.MIplImage;
                MIplImage md = imgDest.MIplImage;

                byte* dataPtrd;
                byte* dataPtro = (byte*)mo.ImageData.ToPointer();
                byte* dataPtrAux;

                nChan = md.NChannels;
                widthStepAux = mo.WidthStep;
                padding = md.WidthStep - md.NChannels * md.Width;

                //centro
                dataPtrd = (byte*)md.ImageData.ToPointer();

                for (y = 0; y < height; ++y)
                {
                    for (x = 0; x < width; ++x)
                    {
                        dataPtrAux = dataPtro + (y) * widthStepAux + (x) * nChan;

                        sumGreen = sumBlue = sumRed = 0;

                        for (i = 0; i < filterSize; ++i)
                        {
                            for (j = 0; j < filterSize; ++j)
                            {
                                sumBlue += matrix[i, j] * dataPtrAux[0];
                                sumGreen += matrix[i, j] * dataPtrAux[1];
                                sumRed += matrix[i, j] * dataPtrAux[2];

                                dataPtrAux += nChan;
                            }
                            dataPtrAux -= nChan * filterSize;
                            dataPtrAux += widthStepAux;
                        }

                        tmp = (sumBlue / matrixWeight) + offset;
                        if (tmp < 0)
                            dataPtrd[0] = 0;
                        else if (tmp > 255)
                            dataPtrd[0] = 255;
                        else
                            dataPtrd[0] = (byte)Math.Round(tmp);

                        tmp = (sumGreen / matrixWeight) + offset;
                        if (tmp < 0)
                            dataPtrd[1] = 0;
                        else if (tmp > 255)
                            dataPtrd[1] = 255;
                        else
                            dataPtrd[1] = (byte)Math.Round(tmp);

                        tmp = (sumRed / matrixWeight) + offset;
                        if (tmp < 0)
                            dataPtrd[2] = 0;
                        else if (tmp > 255)
                            dataPtrd[2] = 255;
                        else
                            dataPtrd[2] = (byte)Math.Round(tmp);

                        dataPtrd += nChan;
                    }
                    dataPtrd += padding;
                }
            }
        }
        public static void Sobel(Image<Bgr, byte> img, Image<Bgr, byte> imgCopy)
        {
            unsafe
            {
                int x, y, i, j, height, width, nChan, padding, widthStep, widthStepAux, filterSize, filterPadding;
                float sumGreenx, sumBluex, sumRedx, sumGreeny, sumBluey, sumRedy;
                float tmp;

                int[,] matrix_vertical = {
                                            {-1, 0, 1},
                                            {-2, 0, 2},
                                            {-1, 0, 1}
                                        };

                int[,] matrix_horizontal = {
                                            {-1, -2, -1},
                                            {0, 0, 0},
                                            {1, 2, 1}
                                        };

                filterSize = 3;
                filterPadding = filterSize / 2;

                height = imgCopy.Height;
                width = imgCopy.Width;

                Image<Bgr, byte> imgAux = new Image<Bgr, byte>(width + filterPadding * 2, height + filterPadding * 2);
                CvInvoke.CopyMakeBorder(imgCopy, imgAux, filterPadding, filterPadding, filterPadding, filterPadding, Emgu.CV.CvEnum.BorderType.Replicate);

                MIplImage mo = imgAux.MIplImage;
                MIplImage md = img.MIplImage;

                byte* dataPtrd;
                byte* dataPtro = (byte*)mo.ImageData.ToPointer();
                byte* dataPtrAux;

                nChan = md.NChannels;
                widthStep = md.WidthStep;
                widthStepAux = mo.WidthStep;
                padding = md.WidthStep - md.NChannels * md.Width;

                //centro
                dataPtrd = (byte*)md.ImageData.ToPointer();

                for (y = 0; y < height; ++y)
                {
                    for (x = 0; x < width; ++x)
                    {
                        dataPtrAux = dataPtro + y * widthStepAux + x * nChan;
                        sumGreeny = sumBluey = sumRedy = 0;

                        for (i = 0; i < filterSize; ++i)
                        {
                            for (j = 0; j < filterSize; ++j)
                            {
                                sumBluey += matrix_vertical[i, j] * dataPtrAux[0];
                                sumGreeny += matrix_vertical[i, j] * dataPtrAux[1];
                                sumRedy += matrix_vertical[i, j] * dataPtrAux[2];

                                dataPtrAux += nChan;
                            }
                            dataPtrAux -= nChan * filterSize;
                            dataPtrAux += widthStepAux;
                        }

                        dataPtrAux = dataPtro + y * widthStepAux + x * nChan;
                        sumGreenx = sumBluex = sumRedx = 0;

                        for (i = 0; i < filterSize; ++i)
                        {
                            for (j = 0; j < filterSize; ++j)
                            {
                                sumBluex += matrix_horizontal[i, j] * dataPtrAux[0];
                                sumGreenx += matrix_horizontal[i, j] * dataPtrAux[1];
                                sumRedx += matrix_horizontal[i, j] * dataPtrAux[2];

                                dataPtrAux += nChan;
                            }
                            dataPtrAux -= nChan * filterSize;
                            dataPtrAux += widthStepAux;
                        }

                        tmp = Math.Abs(sumBluey) + Math.Abs(sumBluex);
                        if (tmp > 255)
                            dataPtrd[0] = 255;
                        else
                            dataPtrd[0] = (byte)Math.Round(tmp);

                        tmp = Math.Abs(sumGreeny) + Math.Abs(sumGreenx);
                        if (tmp > 255)
                            dataPtrd[1] = 255;
                        else
                            dataPtrd[1] = (byte)Math.Round(tmp);

                        tmp = Math.Abs(sumRedy) + Math.Abs(sumRedx);
                        if (tmp > 255)
                            dataPtrd[2] = 255;
                        else
                            dataPtrd[2] = (byte)Math.Round(tmp);

                        dataPtrd += nChan;
                    }
                    dataPtrd += padding;
                }
            }
        }
        public static void Diferentiation(Image<Bgr, byte> imgDest, Image<Bgr, byte> imgOrig)
        {
            unsafe
            {
                int x, y, height, width, nChan, padding, widthStep;
                float sumGreenx, sumBluex, sumRedx, sumGreeny, sumBluey, sumRedy;
                float tmp;

                height = imgOrig.Height;
                width = imgOrig.Width;

                MIplImage mo = imgOrig.MIplImage;
                MIplImage md = imgDest.MIplImage;

                byte* dataPtrd;
                byte* dataPtro = (byte*)mo.ImageData.ToPointer();
                byte* dataPtrAux;

                nChan = md.NChannels;
                widthStep = md.WidthStep;
                padding = md.WidthStep - md.NChannels * md.Width;

                //border direita

                dataPtrd = (byte*)md.ImageData.ToPointer() + nChan * (width - 1);

                for (y = 0; y < height - 1; ++y)
                {
                    dataPtrAux = dataPtro + nChan * (width - 1) + y * widthStep;

                    sumBluex = dataPtrAux[0];
                    sumGreenx = dataPtrAux[1];
                    sumRedx = dataPtrAux[2];

                    dataPtrAux += widthStep;

                    sumBluex -= dataPtrAux[0];
                    sumGreenx -= dataPtrAux[1];
                    sumRedx -= dataPtrAux[2];

                    tmp = Math.Abs(sumBluex);
                    dataPtrd[0] = (byte)(tmp > 255 ? 255 : Math.Round(tmp));

                    tmp = Math.Abs(sumGreenx);
                    dataPtrd[1] = (byte)(tmp > 255 ? 255 : Math.Round(tmp));

                    tmp = Math.Abs(sumRedx);
                    dataPtrd[2] = (byte)(tmp > 255 ? 255 : Math.Round(tmp));

                    dataPtrd += widthStep;
                }

                //border baixo

                dataPtrd = (byte*)md.ImageData.ToPointer() + widthStep * (height - 1);

                for (x = 0; x < width - 1; ++x)
                {
                    dataPtrAux = dataPtro + x * nChan + (height - 1) * widthStep;

                    sumBluex = dataPtrAux[0];
                    sumGreenx = dataPtrAux[1];
                    sumRedx = dataPtrAux[2];

                    dataPtrAux += nChan;

                    sumBluex -= dataPtrAux[0];
                    sumGreenx -= dataPtrAux[1];
                    sumRedx -= dataPtrAux[2];

                    tmp = Math.Abs(sumBluex);
                    dataPtrd[0] = (byte)(tmp > 255 ? 255 : Math.Round(tmp));

                    tmp = Math.Abs(sumGreenx);
                    dataPtrd[1] = (byte)(tmp > 255 ? 255 : Math.Round(tmp));

                    tmp = Math.Abs(sumRedx);
                    dataPtrd[2] = (byte)(tmp > 255 ? 255 : Math.Round(tmp));

                    dataPtrd += nChan;
                }

                //canto inferior esquerdo

                dataPtrd[0] = dataPtrd[1] = dataPtrd[2] = 0;

                dataPtrd = (byte*)md.ImageData.ToPointer();

                for (y = 0; y < height - 1; ++y)
                {
                    for (x = 0; x < width - 1; ++x)
                    {
                        dataPtrAux = dataPtro + y * widthStep + x * nChan;

                        sumBluex = sumBluey = dataPtrAux[0];
                        sumGreenx = sumGreeny = dataPtrAux[1];
                        sumRedx = sumRedy = dataPtrAux[2];

                        dataPtrAux += nChan;

                        sumBluey -= dataPtrAux[0];
                        sumGreeny -= dataPtrAux[1];
                        sumRedy -= dataPtrAux[2];

                        dataPtrAux -= nChan - widthStep;

                        sumBluex -= dataPtrAux[0];
                        sumGreenx -= dataPtrAux[1];
                        sumRedx -= dataPtrAux[2];

                        tmp = Math.Abs(sumBluex) + Math.Abs(sumBluey);
                        dataPtrd[0] = (byte)(tmp > 255 ? 255 : Math.Round(tmp));

                        tmp = Math.Abs(sumGreenx) + Math.Abs(sumGreeny);
                        dataPtrd[1] = (byte)(tmp > 255 ? 255 : Math.Round(tmp));

                        tmp = Math.Abs(sumRedx) + Math.Abs(sumRedy);
                        dataPtrd[2] = (byte)(tmp > 255 ? 255 : Math.Round(tmp));

                        dataPtrd += nChan;
                    }
                    dataPtrd += nChan + padding;
                }
            }
        }
        public static void Roberts(Image<Bgr, byte> imgDest, Image<Bgr, byte> imgOrig)
        {
            unsafe
            {
                int x, y, height, width, nChan, padding, widthStep;
                float sumGreenx, sumBluex, sumRedx, sumGreeny, sumBluey, sumRedy;
                float tmp;

                height = imgOrig.Height;
                width = imgOrig.Width;

                MIplImage mo = imgOrig.MIplImage;
                MIplImage md = imgDest.MIplImage;

                byte* dataPtrd;
                byte* dataPtro = (byte*)mo.ImageData.ToPointer();
                byte* dataPtrAux;

                nChan = md.NChannels;
                widthStep = md.WidthStep;
                padding = md.WidthStep - md.NChannels * md.Width;

                //border direita

                dataPtrd = (byte*)md.ImageData.ToPointer() + nChan * (width - 1);

                for (y = 0; y < height - 1; ++y)
                {
                    dataPtrAux = dataPtro + nChan * (width - 1) + y * widthStep;

                    sumBluex = dataPtrAux[0];
                    sumGreenx = dataPtrAux[1];
                    sumRedx = dataPtrAux[2];

                    dataPtrAux += widthStep;

                    sumBluex -= dataPtrAux[0];
                    sumGreenx -= dataPtrAux[1];
                    sumRedx -= dataPtrAux[2];

                    tmp = Math.Abs(2 * sumBluex);
                    dataPtrd[0] = (byte)(tmp > 255 ? 255 : Math.Round(tmp));

                    tmp = Math.Abs(2 * sumGreenx);
                    dataPtrd[1] = (byte)(tmp > 255 ? 255 : Math.Round(tmp));

                    tmp = Math.Abs(2 * sumRedx);
                    dataPtrd[2] = (byte)(tmp > 255 ? 255 : Math.Round(tmp));

                    dataPtrd += widthStep;
                }

                //border baixo

                dataPtrd = (byte*)md.ImageData.ToPointer() + widthStep * (height - 1);

                for (x = 0; x < width - 1; ++x)
                {
                    dataPtrAux = dataPtro + x * nChan + (height - 1) * widthStep;

                    sumBluex = dataPtrAux[0];
                    sumGreenx = dataPtrAux[1];
                    sumRedx = dataPtrAux[2];

                    dataPtrAux += nChan;

                    sumBluex -= dataPtrAux[0];
                    sumGreenx -= dataPtrAux[1];
                    sumRedx -= dataPtrAux[2];

                    tmp = Math.Abs(2 * sumBluex);
                    dataPtrd[0] = (byte)(tmp > 255 ? 255 : Math.Round(tmp));

                    tmp = Math.Abs(2 * sumGreenx);
                    dataPtrd[1] = (byte)(tmp > 255 ? 255 : Math.Round(tmp));

                    tmp = Math.Abs(2 * sumRedx);
                    dataPtrd[2] = (byte)(tmp > 255 ? 255 : Math.Round(tmp));

                    dataPtrd += nChan;
                }

                //canto inferior esquerdo

                dataPtrd[0] = dataPtrd[1] = dataPtrd[2] = 0;

                //core
                dataPtrd = (byte*)md.ImageData.ToPointer();

                for (y = 0; y < height - 1; ++y)
                {
                    for (x = 0; x < width - 1; ++x)
                    {
                        dataPtrAux = dataPtro + y * widthStep + x * nChan;

                        sumBluex = dataPtrAux[0];
                        sumGreenx = dataPtrAux[1];
                        sumRedx = dataPtrAux[2];

                        dataPtrAux += nChan;

                        sumBluey = dataPtrAux[0];
                        sumGreeny = dataPtrAux[1];
                        sumRedy = dataPtrAux[2];

                        dataPtrAux += widthStep;

                        sumBluex -= dataPtrAux[0];
                        sumGreenx -= dataPtrAux[1];
                        sumRedx -= dataPtrAux[2];

                        dataPtrAux -= nChan;

                        sumBluey -= dataPtrAux[0];
                        sumGreeny -= dataPtrAux[1];
                        sumRedy -= dataPtrAux[2];

                        tmp = Math.Abs(sumBluex) + Math.Abs(sumBluey);
                        dataPtrd[0] = (byte)(tmp > 255 ? 255 : Math.Round(tmp));

                        tmp = Math.Abs(sumGreenx) + Math.Abs(sumGreeny);
                        dataPtrd[1] = (byte)(tmp > 255 ? 255 : Math.Round(tmp));

                        tmp = Math.Abs(sumRedx) + Math.Abs(sumRedy);
                        dataPtrd[2] = (byte)(tmp > 255 ? 255 : Math.Round(tmp));

                        dataPtrd += nChan;
                    }
                    dataPtrd += nChan + padding;
                }
            }
        }

        public static void Median(Image<Bgr, byte> imgDest, Image<Bgr, byte> imgOrig) {
            MIplImage md = imgDest.MIplImage;

            int nChan = md.NChannels;

            CvInvoke.MedianBlur(imgOrig, imgDest, nChan);
        }

        public static int[] Histogram_Gray(Emgu.CV.Image<Bgr, byte> img) {
            unsafe
            {
                int[] hist_arr = new int[256];
                Array.Clear(hist_arr, 0, hist_arr.Length);

                int x, y, height, width, nChan, widthStep, padding, gray;

                height = img.Height;
                width = img.Width;

                MIplImage md = img.MIplImage;
                byte* dataPtrd;

                nChan = md.NChannels;
                widthStep = md.WidthStep;
                padding = md.WidthStep - md.NChannels * md.Width;

                //centro
                dataPtrd = (byte*)md.ImageData.ToPointer();

                for (y = 0; y < height; ++y)
                {
                    for (x = 0; x < width; ++x)
                    {
                        // convert to gray
                        gray = (byte)Math.Round(((int)dataPtrd[0] + dataPtrd[1] + dataPtrd[2]) / 3.0);

                        hist_arr[gray]++;
                        dataPtrd += nChan;
                    }
                    dataPtrd += padding;
                }
                return hist_arr;
            }
        }

        public static int[,] Histogram_RGB(Emgu.CV.Image<Bgr, byte> img)
        {
            unsafe
            {
                int[,] histogram = new int[3, 256];
                Array.Clear(histogram, 0, histogram.Length);

                MIplImage m = img.MIplImage;
                byte* dataPtr = (byte*)m.ImageData.ToPointer(); // Pointer to the image

                int width = img.Width;
                int height = img.Height;
                int nChan = m.NChannels; // number of channels - 3
                int padding = m.WidthStep - m.NChannels * m.Width; // alinhament bytes (padding)
                int x, y;

                if (nChan == 3) // image in RGB
                {
                    for (y = 0; y < height; ++y)
                    {
                        for (x = 0; x < width; ++x)
                        {

                            histogram[0, dataPtr[0]] += 1;
                            histogram[1, dataPtr[1]] += 1;
                            histogram[2, dataPtr[2]] += 1;

                            // advance the pointer to the next pixel
                            dataPtr += nChan;
                        }

                        //at the end of the line advance the pointer by the aligment bytes (padding)
                        dataPtr += padding;
                    }
                }

                return histogram;
            }
        }

        public static int[,] Histogram_All(Emgu.CV.Image<Bgr, byte> img)
        {
            unsafe
            {
                int[,] histogram = new int[4, 256];
                Array.Clear(histogram, 0, histogram.Length);

                MIplImage m = img.MIplImage;
                byte* dataPtr = (byte*)m.ImageData.ToPointer(); // Pointer to the image
                byte gray, blue, green, red;

                int width = img.Width;
                int height = img.Height;
                int nChan = m.NChannels; // number of channels - 3
                int padding = m.WidthStep - m.NChannels * m.Width; // alinhament bytes (padding)
                int x, y;

                if (nChan == 3) // image in RGB
                {
                    for (y = 0; y < height; ++y)
                    {
                        for (x = 0; x < width; ++x)
                        {
                            blue = dataPtr[0];
                            green = dataPtr[1];
                            red = dataPtr[2];
                            gray = (byte)Math.Round((blue + green + red) / 3.0);

                            histogram[0, gray] += 1;

                            histogram[1, blue] += 1;
                            histogram[2, green] += 1;
                            histogram[3, red] += 1;

                            // advance the pointer to the next pixel
                            dataPtr += nChan;
                        }

                        //at the end of the line advance the pointer by the aligment bytes (padding)
                        dataPtr += padding;
                    }
                }
                return histogram;
            }
        }

        public static void ConvertToBW(Emgu.CV.Image<Bgr, byte> img, int threshold)
        {
            unsafe
            {
                int x, y, height, width, nChan, widthStep, padding, gray;

                height = img.Height;
                width = img.Width;

                MIplImage md = img.MIplImage;
                byte* dataPtrd;

                nChan = md.NChannels;
                widthStep = md.WidthStep;
                padding = md.WidthStep - md.NChannels * md.Width;

                //centro
                dataPtrd = (byte*)md.ImageData.ToPointer();

                for (y = 0; y < height; ++y)
                {
                    for (x = 0; x < width; ++x)
                    {
                        // convert to gray
                        gray = (byte)Math.Round(((int)dataPtrd[0] + dataPtrd[1] + dataPtrd[2]) / 3.0);

                        if (gray <= threshold) {
                            dataPtrd[0] = 0;
                            dataPtrd[1] = 0;
                            dataPtrd[2] = 0;
                        }
                        else
                        {
                            dataPtrd[0] = 255;
                            dataPtrd[1] = 255;
                            dataPtrd[2] = 255;
                        }

                        dataPtrd += nChan;
                    }
                    dataPtrd += padding;
                }
            }
        }

        public static void ConvertToBW_Otsu(Emgu.CV.Image<Bgr, byte> img) {
            unsafe
            {
                int x, y, height, width, nChan, widthStep, padding, gray, threshold = 1;

                height = img.Height;
                width = img.Width;

                float totalNPixeis = height * width;

                MIplImage md = img.MIplImage;
                byte* dataPtrd;

                nChan = md.NChannels;
                widthStep = md.WidthStep;
                padding = md.WidthStep - md.NChannels * md.Width;

                double max_sig = 0, sig;

                float q1, q2, u1, u2, p1;

                float[] hist_arr = new float[256];
                Array.Clear(hist_arr, 0, hist_arr.Length);

                //centro
                dataPtrd = (byte*)md.ImageData.ToPointer();

                for (y = 0; y < height; ++y)
                {
                    for (x = 0; x < width; ++x)
                    {
                        // convert to gray
                        gray = (byte)Math.Round(((int)dataPtrd[0] + dataPtrd[1] + dataPtrd[2]) / 3.0);

                        hist_arr[gray]++;
                        dataPtrd += nChan;
                    }
                    dataPtrd += padding;
                }

                for (int i = 0; i < hist_arr.Length; ++i)
                {
                    hist_arr[i] = hist_arr[i] / totalNPixeis;
                }

                for (int t = 1; t < 255; ++t)
                {
                    p1 = 0;
                    u1 = 0;
                    u2 = 0;
                    for (int i = 0; i <= t; ++i)
                    {
                        p1 += hist_arr[i];
                        u1 += i * hist_arr[i];
                    }

                    q1 = p1 / totalNPixeis;
                    q2 = (1 - p1) / totalNPixeis;

                    for (int i = t + 1; i < 255; ++i)
                    {
                        u2 += i * hist_arr[i];
                    }

                    sig = Math.Sqrt(q1 * q2) * Math.Abs(u1 / q1 - u2 / q2);
                    if (sig > max_sig) {
                        max_sig = sig;
                        threshold = t;
                    }

                }
                //centro
                dataPtrd = (byte*)md.ImageData.ToPointer();

                for (y = 0; y < height; ++y)
                {
                    for (x = 0; x < width; ++x)
                    {
                        // convert to gray
                        gray = (byte)Math.Round(((int)dataPtrd[0] + dataPtrd[1] + dataPtrd[2]) / 3.0);

                        if (gray <= threshold)
                        {
                            dataPtrd[0] = 0;
                            dataPtrd[1] = 0;
                            dataPtrd[2] = 0;
                        }
                        else
                        {
                            dataPtrd[0] = 255;
                            dataPtrd[1] = 255;
                            dataPtrd[2] = 255;
                        }

                        dataPtrd += nChan;
                    }
                    dataPtrd += padding;
                }
            }
        }

        private static void Filter4Red(Image<Bgr, byte> imgDest, Image<Hsv, byte> imgHsv) {
            unsafe
            {
                int width = imgDest.Width;
                int height = imgDest.Height;

                MIplImage m = imgDest.MIplImage;
                byte* dataPtr = (byte*)m.ImageData.ToPointer(); // Pointer to the image
                int nChan = m.NChannels; // number of channels = 3
                int padding = m.WidthStep - m.NChannels * m.Width; // alinhament bytes (padding)

                MIplImage m_hsv = imgHsv.MIplImage;
                byte* dataPtr_hsv = (byte*)m_hsv.ImageData.ToPointer(); // Pointer to the image
                int nChan_hsv = m_hsv.NChannels; // number of channels = 3
                int padding_hsv = m_hsv.WidthStep - m_hsv.NChannels * m_hsv.Width; // alinhament bytes (padding)

                int x, y;
                if (nChan == 3) // image in RGB
                {
                    for (y = 0; y < height; y++)
                    {
                        for (x = 0; x < width; x++)
                        {
                          if (!((dataPtr_hsv[1] > 100 && dataPtr_hsv[0] < 10) || (dataPtr_hsv[0] > 160 && dataPtr_hsv[1] > 100 && dataPtr_hsv[0] < 179)))
                            {
                                dataPtr[0] = 0;
                                dataPtr[1] = 0;
                                dataPtr[2] = 0;
                            }

                            // advance the pointer to the next pixel
                            dataPtr += nChan;
                            dataPtr_hsv += nChan_hsv;
                        }

                        //at the end of the line advance the pointer by the aligment bytes (padding)
                        dataPtr += padding;
                        dataPtr_hsv += padding_hsv;
                    }
                }
            }
        }

        private static void joinedComponents(Image<Bgr, byte> imgDest)
        {
            unsafe
            {
                int width = imgDest.Width;
                int height = imgDest.Height;

                Image<Bgr, byte> imgAux = new Image<Bgr, byte>(width + 2, height + 2);
                CvInvoke.CopyMakeBorder(imgDest, imgAux, 1, 1, 1, 1, Emgu.CV.CvEnum.BorderType.Replicate);

                MIplImage mo = imgDest.MIplImage;
                MIplImage m = imgAux.MIplImage;
                byte* dataPtrOrg = (byte*)mo.ImageData.ToPointer();     // Pointer to the image
                int nChan = mo.NChannels;
                int witdhStep = m.WidthStep;
                int padding = mo.WidthStep - nChan * width;  // alinhament bytes (padding)

                byte* dataPtrd;
                byte* dataPtro = (byte*)mo.ImageData.ToPointer();
                byte* dataPtrAux;

                byte* dataPtr = dataPtrOrg;
                int currentTag = 100;
                int tag;
                int x, y;
                byte* pixel_left;
                byte* pixel_top;
                byte* pixel_topR;
                byte* pixel_topL;
                List<(int MinValue, int MaxValue)> colisions = new List<(int, int)>();
                
                for (y = 0; y < height; y++)
                {
                    for (x = 0; x < width; x++)
                    {
                        if (dataPtr[0] == 0 && dataPtr[1] == 0 && dataPtr[2] == 0)
                        {
                            dataPtr += nChan;
                            continue;
                        }

                        pixel_left = dataPtr - nChan;
                        pixel_top = dataPtr - witdhStep;
                        pixel_topL = dataPtr - witdhStep - nChan;
                        pixel_topR = dataPtr - witdhStep + nChan;

                        //new object found (pixel is one  and pixels left and all top is zero)
                        if ((pixel_left[0] == 0 && pixel_left[1] == 0 && pixel_left[2] == 0) &&
                            (pixel_top[0] == 0 && pixel_top[1] == 0 && pixel_top[2] == 0) &&
                            (pixel_topL[0] == 0 && pixel_topL[1] == 0 && pixel_topL[2] == 0) &&
                            (pixel_topR[0] == 0 && pixel_topR[1] == 0 && pixel_topR[2] == 0))
                        {
                            dataPtr[0] = (byte)((currentTag >> 16) & 0xFF);
                            dataPtr[1] = (byte)((currentTag >> 8) & 0xFF);
                            dataPtr[2] = (byte)(currentTag & 0xFF);
                            currentTag += 100;

                            dataPtr += nChan;
                            continue;
                        }

                        //continuation of object found (pixel is one and (pixels left or any of the top are also one))
                        tag = 0;
                        if (pixel_left[0] != 0 || pixel_left[1] != 0 || pixel_left[2] != 0)
                        {
                            tag = pixel_left[0] << 16 | pixel_left[1] << 8 | pixel_left[2];
                        }
                        if (pixel_top[0] != 0 || pixel_top[1] != 0 || pixel_top[2] != 0)
                        {
                            if (tag == 0)
                                tag = pixel_top[0] << 16 | pixel_top[1] << 8 | pixel_top[2];
                            else if (tag != (pixel_top[0] << 16 | pixel_top[1] << 8 | pixel_top[2]))
                            {
                                colisions.Add((
                                    Math.Min(tag, (pixel_top[0] << 16) | (pixel_top[1] << 8) | pixel_top[2]),
                                    Math.Max(tag, (pixel_top[0] << 16) | (pixel_top[1] << 8) | pixel_top[2])
                                ));
                                tag = Math.Min(tag, (pixel_top[0] << 16) | (pixel_top[1] << 8) | pixel_top[2]);
                                dataPtr[0] = (byte)((tag >> 16) & 0xFF);
                                dataPtr[1] = (byte)((tag >> 8) & 0xFF);
                                dataPtr[2] = (byte)(tag & 0xFF);

                                dataPtr += nChan;
                                continue;
                            }
                        }
                        if (pixel_topL[0] != 0 || pixel_topL[1] != 0 || pixel_topL[2] != 0)
                        {
                            if (tag == 0)
                                tag = pixel_topL[0] << 16 | pixel_topL[1] << 8 | pixel_topL[2];
                            else if (tag != (pixel_topL[0] << 16 | pixel_topL[1] << 8 | pixel_topL[2]))
                            {
                                colisions.Add((
                                    Math.Min(tag, (pixel_topL[0] << 16) | (pixel_topL[1] << 8) | pixel_topL[2]),
                                    Math.Max(tag, (pixel_topL[0] << 16) | (pixel_topL[1] << 8) | pixel_topL[2])
                                ));
                                tag = Math.Min(tag, (pixel_topL[0] << 16) | (pixel_topL[1] << 8) | pixel_topL[2]);
                                dataPtr[0] = (byte)((tag >> 16) & 0xFF);
                                dataPtr[1] = (byte)((tag >> 8) & 0xFF);
                                dataPtr[2] = (byte)(tag & 0xFF);
                                dataPtr += nChan;
                                continue;
                            }
                        }
                        if (pixel_topR[0] != 0 || pixel_topR[1] != 0 || pixel_topR[2] != 0)
                        {
                            if (tag == 0)
                                tag = pixel_topR[0] << 16 | pixel_topR[1] << 8 | pixel_topR[2];
                            else if (tag != (pixel_topR[0] << 16 | pixel_topR[1] << 8 | pixel_topR[2]))
                            {
                                colisions.Add((
                                        Math.Min(tag, (pixel_topR[0] << 16) | (pixel_topR[1] << 8) | pixel_topR[2]),
                                        Math.Max(tag, (pixel_topR[0] << 16) | (pixel_topR[1] << 8) | pixel_topR[2])
                                ));
                                tag = Math.Min(tag, (pixel_topR[0] << 16) | (pixel_topR[1] << 8) | pixel_topR[2]);
                                dataPtr[0] = (byte)((tag >> 16) & 0xFF);
                                dataPtr[1] = (byte)((tag >> 8) & 0xFF);
                                dataPtr[2] = (byte)(tag & 0xFF);
                                dataPtr += nChan;
                                continue;
                            }
                        }
                        dataPtr[0] = (byte)((tag >> 16) & 0xFF);
                        dataPtr[1] = (byte)((tag >> 8) & 0xFF);
                        dataPtr[2] = (byte)(tag & 0xFF);
                        dataPtr += nChan;
                    }
                    dataPtr += padding;
                }

                /*TODO: fix shitty fix for repetetition of collisions that should never happen*/
                //colisions = colisions.Distinct().ToList();

                for (int i = 0; i < colisions.Count; i++)
                {
                    for (int j = 0; j < colisions.Count; j++)
                    {
                        if (colisions[j].MinValue == colisions[i].MaxValue)
                        {
                            colisions[j] = (colisions[i].MinValue, colisions[j].MaxValue);
                        }
                    }
                }

                dataPtr = dataPtrOrg;
                //TODO: worry about the borders
                for (y = 1; y < height - 1; y++)
                {
                    for (x = 1; x < width - 1; x++)
                    {
                        if (dataPtr[0] == 0 && dataPtr[1] == 0 && dataPtr[2] == 0)
                        {
                            dataPtr += nChan;
                            continue;
                        }

                        tag = (dataPtr[0] << 16) | (dataPtr[1] << 8) | dataPtr[2];

                        var replaceVal = colisions.FirstOrDefault(c => c.MaxValue == tag);
                        if (replaceVal != default)
                        {
                            dataPtr[0] = (byte)((replaceVal.MinValue >> 16) & 0xFF); // Red channel
                            dataPtr[1] = (byte)((replaceVal.MinValue >> 8) & 0xFF);  // Green channel
                            dataPtr[2] = (byte)(replaceVal.MinValue & 0xFF);         // Blue channel
                        }

                        dataPtr += nChan;
                    }
                    dataPtr += padding;
                }

            }
        }

        private static Dictionary<(byte B, byte G, byte R), ObjectParams> removeSmallAreas(Image<Bgr, byte> imgDest, int area)
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
        private static void getcoords(Image<Bgr, byte> imgDest, Dictionary<(byte B, byte G, byte R), ObjectParams> objects)
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
                int top = -1, left = -1, bottom = -1, right = -1;
                byte* dataPtr = dataPtrOrg;

                foreach (var sing_object in objects)
                {
                    dataPtr = dataPtrOrg;
                    //loop lines first
                    for (y = 0; y < height; y++)
                    {
                        for (x = 0; x < width; x++)
                        {
                            if (dataPtr[0] == sing_object.Value.Blue && dataPtr[1] == sing_object.Value.Green && dataPtr[2] == sing_object.Value.Red)
                            {
                                top = y;
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
                            if (dataPtr[0] == sing_object.Value.Blue && dataPtr[1] == sing_object.Value.Green && dataPtr[2] == sing_object.Value.Red)
                            {
                                left = x;
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
                            if (dataPtr[0] == sing_object.Value.Blue && dataPtr[1] == sing_object.Value.Green && dataPtr[2] == sing_object.Value.Red)
                            {
                                bottom = y;
                                x = width; //force exit of for loops
                                y = 0;
                            }
                            dataPtr -= nChan;
                        }
                        dataPtr -= padding;
                    }
                    if (dataPtr[0] != 0 && bottom == -1)
                    {
                        bottom = y;
                    }

                    // Loop through columns first starting at right
                    for (x = width; x > 0; x--)
                    {
                        dataPtr = dataPtrOrg + x * nChan;
                        for (y = 0; y < height; y++)
                        {
                            if (dataPtr[0] == sing_object.Value.Blue && dataPtr[1] == sing_object.Value.Green && dataPtr[2] == sing_object.Value.Red)
                            {
                                right = x;
                                x = 0; //force exit of for loops
                                y = height;
                            }

                            dataPtr += width * nChan + padding;
                        }
                    }
                    if (top == -1 || left == -1 || right == -1 || bottom == -1)
                        throw new InvalidOperationException("One or more values are -1, which is not allowed.");
                    objects[(sing_object.Key.B, sing_object.Key.G, sing_object.Key.R)].Top = top;
                    objects[(sing_object.Key.B, sing_object.Key.G, sing_object.Key.R)].Bottom = bottom;
                    objects[(sing_object.Key.B, sing_object.Key.G, sing_object.Key.R)].Right = right;
                    objects[(sing_object.Key.B, sing_object.Key.G, sing_object.Key.R)].Left = left;
                 }
            }
        }
        private static void calculateCircularity(Image<Bgr, byte> imgDest, Dictionary<(byte B, byte G, byte R), ObjectParams> objects)
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
                byte* dataPtr = dataPtrOrg;

                int centerX;
                int centerY;

                int radiosleft;
                int radiosright;
                int radiostop;
                int radiosbottom;
                double diameter;
                int B;
                int G;
                int R;

                foreach (var poss_sings in objects)
                {
                    centerX = poss_sings.Value.Right - poss_sings.Value.Left;
                    centerY = poss_sings.Value.Bottom - poss_sings.Value.Top;
                    poss_sings.Value.CenterX = centerX;
                    poss_sings.Value.CenterY = centerY;

                    B = poss_sings.Key.B;
                    G = poss_sings.Key.G;
                    R = poss_sings.Key.R;

                    radiosleft = 0;
                    radiosright = 0;
                    radiostop = 0;
                    radiosbottom = 0;
                    dataPtr = dataPtrOrg + centerX * nChan + centerY* widthStep;
                    for (y = centerY; y < poss_sings.Value.Bottom; y++)
                    {
                        if (dataPtr[0] == B && dataPtr[1] == G && dataPtr[2] == R)
                            y = height; //force exit
                        else
                        {
                            dataPtr += widthStep;
                            radiosbottom++;
                        }
                    }

                    dataPtr = dataPtrOrg + centerX * nChan + centerY * widthStep;
                    for (y = centerY; y > poss_sings.Value.Top; y--)
                    {
                        if (dataPtr[0] == B && dataPtr[1] == G && dataPtr[2] == R)
                            y = 0; //force exit
                        else
                        {
                            dataPtr += widthStep;
                            radiostop++;
                        }
                    }

                    dataPtr = dataPtrOrg + centerX * nChan + centerY * widthStep;
                    for (x = centerX; x < poss_sings.Value.Right; x++)
                    {
                        if (dataPtr[0] == B && dataPtr[1] == G && dataPtr[2] == R)
                            x = width; //force exit
                        else
                        {
                            dataPtr += nChan;
                            radiosright++;
                        }
                    }

                    dataPtr = dataPtrOrg + centerX * nChan + centerY * widthStep;
                    for (x = centerX; x > poss_sings.Value.Left; x--)
                    {
                        if (dataPtr[0] == B && dataPtr[1] == G && dataPtr[2] == R)
                            x = 0; //force exit
                        else
                        {
                            dataPtr += nChan;
                            radiosleft++;
                        }
                    }

                    diameter = (radiosbottom + radiosright + radiosleft + radiostop) / 2;
                    poss_sings.Value.diamter = diameter;
                    poss_sings.Value.Circularity = (4 * poss_sings.Value.Area )/(diameter * diameter);
                }

                //It is not possible to remove items while iterating
                //so this list was made
                // Create a list to store keys to remove
                var keysToRemove = objects
                    .Where(kvp => kvp.Value.Circularity < 0.05)
                    .Select(kvp => kvp.Key)
                    .ToList();

                // Remove the keys from the dictionary
                foreach (var key in keysToRemove)
                {
                    objects.Remove(key);
                }

            }
        }

        private static void filter4Black(Image<Bgr, byte> imgDest, Image<Hsv, byte> imgHsv)
        {
            unsafe
            {
                int width = imgDest.Width;
                int height = imgDest.Height;

                MIplImage m = imgDest.MIplImage;
                byte* dataPtr = (byte*)m.ImageData.ToPointer(); // Pointer to the image
                int nChan = m.NChannels; // number of channels = 3
                int padding = m.WidthStep - m.NChannels * m.Width; // alinhament bytes (padding)

                MIplImage m_hsv = imgHsv.MIplImage;
                byte* dataPtr_hsv = (byte*)m_hsv.ImageData.ToPointer(); // Pointer to the image
                int nChan_hsv = m_hsv.NChannels; // number of channels = 3
                int padding_hsv = m_hsv.WidthStep - m_hsv.NChannels * m_hsv.Width; // alinhament bytes (padding)

                int x, y;
                if (nChan == 3) // image in RGB
                {
                    for (y = 0; y < height; y++)
                    {
                        for (x = 0; x < width; x++)
                        {
                            if (!(dataPtr_hsv[2] <= 70))
                            {
                                dataPtr[0] = 0; // Blue channel
                                dataPtr[1] = 0; // Green channel
                                dataPtr[2] = 0; // Red channel
                            }

                            // advance the pointer to the next pixel
                            dataPtr += nChan;
                            dataPtr_hsv += nChan_hsv;
                        }

                        //at the end of the line advance the pointer by the aligment bytes (padding)
                        dataPtr += padding;
                        dataPtr_hsv += padding_hsv;
                    }
                }
            }
        }


        /// <summary>
        /// Convert to Hsv
        /// Direct access to memory - faster method
        /// </summary>
        /// <param name="img">image</param>
        public static void ConvertToHsv(Image<Bgr, byte> imgOrig, Image<Hsv, byte> imgDest)
        {
            unsafe
            {
                // direct access to the image memory(sequencial)
                // direcion top left -> bottom right

                MIplImage mo = imgOrig.MIplImage;
                byte* dataPtro = (byte*)mo.ImageData.ToPointer(); // Pointer to the image

                MIplImage md = imgDest.MIplImage;
                byte* dataPtrd = (byte*)md.ImageData.ToPointer();

                byte* dataPtraux;

                byte blue, green, red, hue, sat, val, del, hconst;

                int width = imgDest.Width;
                int height = imgDest.Height;
                int nChan = md.NChannels; // number of channels = 3
                int padding = md.WidthStep - md.NChannels * md.Width; // alinhament bytes (padding)
                int widthStepO = mo.WidthStep;
                int nChanO = mo.NChannels;
                int x, y;

                if (nChan == 3) return; // image in RGB

                for (y = 0; y < height; y++)
                {
                    for (x = 0; x < width; x++)
                    {

                        dataPtraux = dataPtro + y * widthStepO + x * nChanO;

                        // store in the image
                        blue = dataPtraux[0];
                        green = dataPtraux[1];
                        red = dataPtraux[2];

                        if (blue > green && blue > red) {
                            val = blue;
                            hconst = (byte) (red - green);

                            if (green > red) {
                                del = (byte) (val - red);
                                sat = (byte) Math.Round((float) del / val);
                                hue = (byte) (4 + Math.Round((float) hconst / del));
                            } else {
                                del = (byte) (val - green);
                                sat = (byte) Math.Round((float)del / val);
                                hue = (byte) (4 + Math.Round((float) hconst / del));
                            }
                        } else if (green > blue && green > red) {
                            val = green;
                            hconst = (byte) (blue - red);

                            if (blue > red) {
                                del = (byte) (val - red);
                                sat = (byte) Math.Round((float) del / val);
                                hue = (byte) (2 + Math.Round((float) hconst / del));
                            }
                            else {
                                del = (byte) (val - blue);
                                sat = (byte) Math.Round((float) del / val);
                                hue = (byte) (2 + Math.Round((float) hconst / del));
                            }
                        } else {
                            val = red;
                            hconst = (byte) (green - blue);

                            if (blue > green) {
                                del = (byte) (val - green);
                                sat = (byte) Math.Round((float) del / val);
                                hue = (byte) Math.Round((float) hconst / del);
                            }
                            else {
                                del = (byte) (val - blue);
                                sat = (byte) Math.Round((float) del / val);
                                hue = (byte) Math.Round((float) hconst / del);
                            }
                        }

                        dataPtrd[0] = hue;
                        dataPtrd[1] = sat;
                        dataPtrd[2] = val;

                        // advance the pointer to the next pixel
                        dataPtrd += nChan;
                    }

                    //at the end of the line advance the pointer by the aligment bytes (padding)
                    dataPtrd += padding;
                }
            }
        }

        public static int FindNum(Image<Bgr, byte> img)
        {
            unsafe
            {
                String cur_path = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                Console.WriteLine(cur_path);
                String[] reference = new string[10];

                for (int i = 0; i < 10; ++i)
                {
                    reference[i] = cur_path;
                }

                Image<Bgr, byte> img2, img_refactor;

                int height1, height2, width1, width2, num;
                float result, res;

                num = 0;
                result = 0;

                for (int i = 0; i < 10; ++i)
                {
                    img2 = new Image<Bgr, byte>(reference[i]);
                    height1 = img.Height;
                    height2 = img2.Height;
                    width1 = img.Width;
                    width2 = img2.Width;

                    img_refactor = img2.Copy();

                    Scale(img_refactor, img, (height1 - height2)/(width1 - width2));
                    ConvertToBW_Otsu(img_refactor);
                    ConvertToBW_Otsu(img2);

                    res = CompImg(img_refactor, img2);

                    if (result > res) {
                        num = i;
                        result = res;
                    }
                }

                return num;
            }
        }

        // comparação de duas imagens do mesmo tamanho
        // quanto mais parecidas as duas imagens mais perto o resultado é de 1
        public static float CompImg(Image<Bgr, byte> img1, Image<Bgr, byte> img2)
        {
            unsafe
            {
                // direct access to the image memory(sequencial)
                // direcion top left -> bottom right

                MIplImage mo = img1.MIplImage;
                byte* dataPtr1 = (byte*)mo.ImageData.ToPointer(); // Pointer to the image

                MIplImage md = img2.MIplImage;
                byte* dataPtr2 = (byte*)md.ImageData.ToPointer();

                int inter, union;
                inter = union = 0;

                bool d1, d2;

                int width = img2.Width;
                int height = img2.Height;
                int nChan = md.NChannels; // number of channels = 3
                int padding = md.WidthStep - md.NChannels * md.Width; // alinhament bytes (padding)
                int widthStepO = mo.WidthStep;
                int nChanO = mo.NChannels;
                int x, y;

                if (nChan == 3) return 0; // image in RGB

                for (y = 0; y < height; y++)
                {
                    for (x = 0; x < width; x++)
                    {

                        d1 = dataPtr1[0] == 0;
                        d2 = dataPtr2[0] == 0;

                        if (d1) {
                            ++union;
                            if (d2) {
                                ++inter;
                            }
                        } else if (d2) {
                            ++union;
                        }

                        // advance the pointer to the next pixel
                        dataPtr1 += nChan;
                        dataPtr2 += nChan;
                    }

                    //at the end of the line advance the pointer by the aligment bytes (padding)
                    dataPtr1 += padding;
                    dataPtr2 += padding;
                }
                
                return inter / union;
            }
        }

        /// <summary>
        /// Sinal Reader
        /// </summary>
        /// <param name="imgDest">imagem de destino (cópia da original)</param>
        /// <param name="imgOrig">imagem original </param>
        /// <param name="level">nivel de dificuldade da imagem</param>
        /// <param name="sinalResult">Objecto resultado - lista de sinais e respectivas informaçoes</param>
        public static void SinalReader(Image<Bgr, byte> imgDest, Image<Bgr, byte> imgOrig, int level, out Results sinalResult){
            unsafe
            {
                int width = imgOrig.Width;
                int height = imgOrig.Height;

                Image<Bgr, byte> imgCopy = imgOrig.Copy();

                MIplImage m = imgDest.MIplImage;
                byte* dataPtr = (byte*)m.ImageData.ToPointer(); // Pointer to the image
                int nChan = m.NChannels; // number of channels = 3
                int padding = m.WidthStep - m.NChannels * m.Width; // alinhament bytes (padding)

                sinalResult = new Results();

                //Sinal creation
                Sinal sinal = new Sinal();
                sinal.sinalEnum = ResultsEnum.sinal_limite_velocidade;

                Digito digito = new Digito();
                digito.digitoRect = new Rectangle(20, 30, 100, 40);
                digito.digito = "1";
                sinal.digitos.Add(digito);

                Digito digito2 = new Digito();
                digito2.digitoRect = new Rectangle(80, 30, 100, 40);
                digito2.digito = "1";
                sinal.digitos.Add(digito2);

                // Convert the image from BGR to HSV color space
                Image<Hsv, byte> imgHsv = new Image<Hsv, byte>(width, height);
                ConvertToHsv(imgOrig, imgHsv);

                Filter4Red(imgCopy, imgHsv);

                joinedComponents(imgCopy);

                Dictionary<(byte B, byte G, byte R), ObjectParams> objects = removeSmallAreas(imgCopy, 1000);

                getcoords(imgCopy, objects);

                calculateCircularity(imgCopy, objects);
               
                imgCopy = imgOrig.Copy();
                foreach (var tag in objects)
                {   
                    int left = tag.Value.Left;
                    int right = tag.Value.Right;
                    int top = tag.Value.Top;    
                    int bottom = tag.Value.Bottom;

                    // Define the cropping rectangle based on the desired coordinates
                    Rectangle cropRect = new Rectangle(left, top, right - left, bottom - top);

                    // Set the ROI (Region of Interest) of the original image to the crop rectangle
                    imgCopy.ROI = cropRect;

                    // Create a new image to store the cropped region
                    Image<Bgr, byte> croppedImage = imgCopy.Copy();

                    // Reset the ROI of the original image to avoid affecting further operations
                    imgCopy.ROI = Rectangle.Empty;

                    Image<Hsv, byte> croppedimgHsv = new Image<Hsv, byte>(croppedImage.Width, croppedImage.Height);
                    CvInvoke.CvtColor(croppedImage, croppedimgHsv, ColorConversion.Bgr2Hsv);

                    filter4Black(croppedImage, croppedimgHsv);

                    joinedComponents(croppedImage);

                    Dictionary<(byte B, byte G, byte R), ObjectParams> numbers = removeSmallAreas(croppedImage, 400);

                    getcoords(croppedImage, numbers);

                    // Place the cropped and modified region back in its original location
                    croppedImage.CopyTo(imgDest.GetSubRect(cropRect));

                    foreach (var number in numbers)
                    {
                        sinal.sinalRect = new Rectangle(left + number.Value.Left, top + number.Value.Top, number.Value.Right - number.Value.Left,number.Value.Bottom - number.Value.Top);
                        // Define the cropping rectangle based on the desired coordinates
                        cropRect = new Rectangle(left, top, right - left, bottom - top);

                        // Set the ROI (Region of Interest) of the original image to the crop rectangle
                        imgCopy.ROI = cropRect;

                        // Create a new image to store the cropped region
                        Image<Bgr, byte> numImg = imgCopy.Copy();
                    }
                    
                    sinal.sinalRect = new Rectangle(tag.Value.Left, tag.Value.Top, tag.Value.Right - tag.Value.Left, tag.Value.Bottom - tag.Value.Top);

                    imgDest.Draw(sinal.sinalRect, new Bgr(Color.Green));
                }
               
                // add sinal to results
                sinalResult.results.Add(sinal);
            }
        }

    }
}
