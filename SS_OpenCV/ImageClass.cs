using System;
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
                byte* dataPtrOrg = dataPtr;
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

                dataPtr = dataPtrOrg + height * (width * nChan);
                for (y = height; y > 0; y--)
                {
                    for (x = 0; x < width; x++)
                    {
                        dataPtr[0] = 255;
                        dataPtr[1] = 255;
                        dataPtr[2] = 255;
                        dataPtr -= nChan;
                    }
                    dataPtr -= padding;
                }
                dataPtr[0] = 255;
                dataPtr[1] = 255;
                dataPtr[2] = 255;
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
                            if (blue < 0)
                                blue = 0;
                            dataPtr[0] = (byte)blue;

                            green = (int)Math.Round(contrast * dataPtr[1] + bright);
                            if (green > 255)
                                green = 255;
                            if (green < 0)
                                green = 0;
                            dataPtr[1] = (byte)green;

                            red = (int)Math.Round(contrast * dataPtr[2] + bright);
                            if (red > 255)
                                red = 255;
                            if (red < 0)
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
                if (nChan == 3) // image in RGB
                {
                    for (y = 0; y < height; y++)
                    {
                        for (x = 0; x < width; x++)
                        {
                            xOrigin = (int)Math.Round((x - (width / 2.0)) * Math.Cos(radAngle) - ((height / 2.0) - y) * Math.Sin(radAngle) + (width / 2.0));
                            yOrigin = (int)Math.Round((height / 2.0) - (x - (width / 2.0)) * Math.Sin(radAngle) - ((height / 2.0) - y) * Math.Cos(radAngle));
                            if ((xOrigin < 0 || yOrigin < 0) || (xOrigin >= width || yOrigin >= height)) {
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
                int x, y, i, j, height, width, nChan, padding, widthStep, widthStepAux, filterSize, filterPadding;
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
                widthStep = md.WidthStep;
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

        private static void getRedSignalOutline(Image<Bgr, byte> imgDest, Image<Hsv, byte> imgHsv) {
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
                            if (!((((dataPtr_hsv[1] > 100) && (dataPtr_hsv[1] > 100)) && (dataPtr_hsv[0] < 10)) || (((dataPtr_hsv[0] > 160) && (dataPtr_hsv[1] > 100) && (dataPtr_hsv[1] > 100)) && (dataPtr_hsv[0] < 179))))
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

        private static int[] getRedSignalOutlineCoords(Image<Bgr, byte>  imgDest)
        {
            unsafe
            {
                int width = imgDest.Width;
                int height = imgDest.Height;

                MIplImage m = imgDest.MIplImage;
                byte* dataPtr = (byte*)m.ImageData.ToPointer();     // Pointer to the image
                int nChan = m.NChannels;                            // number of channels = 3
                int padding = m.WidthStep - m.NChannels * m.Width;  // alinhament bytes (padding)

                int x, y;
                int top = -1, left = -1, bottom = -1, right = -1;
                byte* dataPtrOrg = dataPtr;
                if (nChan == 3) 
                {
                    //loop lines first
                    for (y = 0; y < height; y++)
                    {
                        for (x = 0; x < width; x++)
                        {
                            if (dataPtr[2] != 0)
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
                            if (dataPtr[2] != 0)
                            {
                                left = x;
                                x = width; //force exit of for loops
                                y = height;
                            }

                            dataPtr += width * nChan + padding;
                        }
                    }

                    dataPtr = dataPtrOrg + height * (width * nChan);
                    for (y = height; y > 0; y--)
                    {
                        for (x = 0; x < width; x++)
                        {
                            if (dataPtr[2] != 0)
                            {
                                bottom = y;
                                x = width; //force exit of for loops
                                y = 0;
                            }
                            dataPtr -= nChan;
                        }
                        dataPtr -= padding;
                    }
                    if (dataPtr[0] != 0 && bottom == -1){
                        bottom = y;
                    }

                    // Loop through columns first starting at right
                    for (x = width; x > 0; x--)
                    {
                        dataPtr = dataPtrOrg + x * nChan;
                        for (y = 0; y < height; y++)
                        {
                            if (dataPtr[2] != 0)
                            {
                                right = x;
                                x = 0; //force exit of for loops
                                y = height;
                            }

                            dataPtr += width * nChan + padding;
                        }
                    }
                }
                return new int[] { top, left, bottom, right };
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
                CvInvoke.CvtColor(imgOrig, imgHsv, ColorConversion.Bgr2Hsv);

                getRedSignalOutline(imgDest, imgHsv);

                ConvertToBW_Otsu(imgDest);

                int[] result = getRedSignalOutlineCoords(imgDest);

                sinal.sinalRect = new Rectangle(result[0], result[1], result[3] - result[1], result[2] - result[0]);

                imgDest.Draw(sinal.sinalRect, new Bgr(Color.Green));

                // add sinal to results
                sinalResult.results.Add(sinal);
            }
        }

    }
}
    //mean get the value of the 9 and then throug everthing off and now calculate the next 9
    //mean_B get the value of the 9 then keep the memory of 6 and add the new 3 values (note: get the some the
    //9 values then sub the last 3 and add the new 3)
    //mean_C prof did not explain...
    //mean and maen_b are 3x3 the mean_c needs to accepst a generic size XxX
    //seperate the code in core, borders and corners
