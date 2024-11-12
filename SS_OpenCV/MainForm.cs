using System;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using ResultsDLL;

namespace SS_OpenCV
{ 
    public partial class MainForm : Form
    {
        Image<Bgr, Byte> img = null; // working image
        Image<Bgr, Byte> imgUndo = null; // undo backup image - UNDO
        string title_bak = "";

        public MainForm()
        {
            InitializeComponent();
            title_bak = Text;
        }

        /// <summary>
        /// Opens a new image
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                img = new Image<Bgr, byte>(openFileDialog1.FileName);
                Text = title_bak + " [" +
                        openFileDialog1.FileName.Substring(openFileDialog1.FileName.LastIndexOf("\\") + 1) +
                        "]";
                imgUndo = img.Copy();
                ImageViewer.Image = img;
                ImageViewer.Refresh();
            }
        }

        /// <summary>
        /// Saves an image with a new name
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                ImageViewer.Image.Save(saveFileDialog1.FileName);
            }
        }

        /// <summary>
        /// Closes the application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// restore last undo copy of the working image
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (imgUndo == null) // verify if the image is already opened
                return; 
            Cursor = Cursors.WaitCursor;
            img = imgUndo.Copy();

            ImageViewer.Image = img;
            ImageViewer.Refresh(); // refresh image on the screen

            Cursor = Cursors.Default; // normal cursor 
        }

        /// <summary>
        /// Change visualization mode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void autoZoomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // zoom
            if (autoZoomToolStripMenuItem.Checked)
            {
                ImageViewer.SizeMode = PictureBoxSizeMode.Zoom;
                ImageViewer.Dock = DockStyle.Fill;
            }
            else // with scroll bars
            {
                ImageViewer.Dock = DockStyle.None;
                ImageViewer.SizeMode = PictureBoxSizeMode.AutoSize;
            }
        }

        /// <summary>
        /// Show authors form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void autoresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AuthorsForm form = new AuthorsForm();
            form.ShowDialog();
        }

        /// <summary>
        /// Calculate the image negative
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void negativeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (img == null) // verify if the image is already opened
                return;
            Cursor = Cursors.WaitCursor; // clock cursor 

            //copy Undo Image
            imgUndo = img.Copy();

            ImageClass.Negative(img);

            ImageViewer.Image = img;
            ImageViewer.Refresh(); // refresh image on the screen

            Cursor = Cursors.Default; // normal cursor 
        }

        /// <summary>
        /// Call image convertion to gray scale
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (img == null) // verify if the image is already opened
                return;
            Cursor = Cursors.WaitCursor; // clock cursor 

            //copy Undo Image
            imgUndo = img.Copy();

            ImageClass.ConvertToGray(img);

            ImageViewer.Image = img;
            ImageViewer.Refresh(); // refresh image on the screen

            Cursor = Cursors.Default; // normal cursor 
        }

        /// <summary>
        /// Call automated image processing check
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void evalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EvalForm eval = new EvalForm();
            eval.ShowDialog();

        }

        private void ImageViewer_MouseMove(object sender, MouseEventArgs e)
        {
            int aux_x = 0;
            int aux_y = 0;
            if (ImageViewer.SizeMode == PictureBoxSizeMode.Zoom)
            {
                aux_x = (int)(e.X / ImageViewer.ZoomScale + ImageViewer.HorizontalScrollBar.Value * ImageViewer.ZoomScale);
                aux_y = (int)(e.Y / ImageViewer.ZoomScale + ImageViewer.VerticalScrollBar.Value * ImageViewer.ZoomScale);

            }
            else
            {
                aux_x = (int)(e.X / ImageViewer.ZoomScale + ImageViewer.HorizontalScrollBar.Value * ImageViewer.ZoomScale);
                aux_y = (int)(e.Y / ImageViewer.ZoomScale + ImageViewer.VerticalScrollBar.Value * ImageViewer.ZoomScale);
            }


            if (img != null && aux_y < img.Height && aux_x < img.Width)
                statusLabel.Text = "X:" + aux_x + "  Y:" + aux_y + " - BGR = (" + img.Data[aux_y, aux_x, 0] + "," + img.Data[aux_y, aux_x, 1] + "," + img.Data[aux_y, aux_x, 2] + ")";

        }

        /// <summary>
        /// Call image RedChannel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void redChannelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (img == null) // verify if the image is already opened
                return;
            Cursor = Cursors.WaitCursor; // clock cursor
                
            //copy Undo Image
            imgUndo = img.Copy();
            
            ImageClass.RedChannel(img);

            ImageViewer.Image = img;
            ImageViewer.Refresh(); // refresh image on the screen

            Cursor = Cursors.Default; // normal cursor 
        }

        /// <summary>
        /// Call image BrightChannel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void brightChannelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (img == null) // verify if the image is already opened
                return;

            //copy Undo Image
            imgUndo = img.Copy();
            int bright;
            double contrast;
            InputBox form;

            while (true)
            {
                form = new InputBox("brilho?");
                form.ShowDialog();
                bright = Convert.ToInt32(form.ValueTextBox.Text);
                if (Math.Abs(bright) > 255)
                    continue;
                break;
            }

            while (true)
            {
                form = new InputBox("contraste?");
                form.ShowDialog();
                contrast = Convert.ToDouble(form.ValueTextBox.Text);
                if (contrast > 3)
                    continue;
                if (contrast < 0)
                    continue;
                break;
            }

            Cursor = Cursors.WaitCursor; // clock cursor
            ImageClass.BrightContrast(img, bright, contrast);

            ImageViewer.Image = img;
            ImageViewer.Refresh(); // refresh image on the screen

            Cursor = Cursors.Default; // normal cursor 
        }

        /// <summary>
        /// Call image Rotation (inverse)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rotationToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (img == null) // verify if the image is already opened
                return;

            //copy Undo Image
            imgUndo = img.Copy();
            Image<Bgr, Byte>  imgCopy = img.Copy();
            double angle;
            InputBox form;

            while (true)
            {
                form = new InputBox("contraste?");
                form.ShowDialog();
                angle = Convert.ToDouble(form.ValueTextBox.Text);
                if (Math.Abs(angle) > Math.PI)
                    continue;
                break;
            }



            Cursor = Cursors.WaitCursor; // clock cursor
            ImageClass.Rotation(img, imgCopy, (float)angle);

            ImageViewer.Image = img;
            ImageViewer.Refresh(); // refresh image on the screen

            Cursor = Cursors.Default; // normal cursor 
        }

        private void translationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (img == null) // verify if the image is already opened
                return;

            //copy Undo Image
            imgUndo = img.Copy();
            Image<Bgr, Byte> imgCopy = img.Copy();
            int dx, dy;
            InputBox form;

            
            form = new InputBox("Dx?");
            form.ShowDialog();
            dx = (int)Convert.ToInt64(form.ValueTextBox.Text);
            
            form = new InputBox("Dy?");
            form.ShowDialog();
            dy = (int)Convert.ToInt64(form.ValueTextBox.Text);

            Cursor = Cursors.WaitCursor; // clock cursor
            ImageClass.Translation(img, imgCopy, dx, dy);

            ImageViewer.Image = img;
            ImageViewer.Refresh(); // refresh image on the screen

            Cursor = Cursors.Default; // normal cursor 
        }

        private void zoom00ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (img == null) // verify if the image is already opened
                return;

            //copy Undo Image
            imgUndo = img.Copy();
            Image<Bgr, Byte> imgCopy = img.Copy();
            double scaleFactor;
            InputBox form;


            form = new InputBox("Scale?");
            form.ShowDialog();
            scaleFactor = Convert.ToDouble(form.ValueTextBox.Text);


            Cursor = Cursors.WaitCursor; // clock cursor
            ImageClass.Scale(img, imgCopy, (float)scaleFactor);

            ImageViewer.Image = img;
            ImageViewer.Refresh(); // refresh image on the screen

            Cursor = Cursors.Default; // normal cursor 
        }

        private void zoomXYToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (img == null) // verify if the image is already opened
                return;

            //copy Undo Image
            imgUndo = img.Copy();
            Image<Bgr, Byte> imgCopy = img.Copy();
            double scaleFactor;
            int x, y;
            InputBox form;

            form = new InputBox("Scale?");
            form.ShowDialog();
            scaleFactor = Convert.ToDouble(form.ValueTextBox.Text);

            form = new InputBox("X?");
            form.ShowDialog();
            x = (int)Convert.ToInt64(form.ValueTextBox.Text);

            form = new InputBox("Y?");
            form.ShowDialog();
            y = (int)Convert.ToInt64(form.ValueTextBox.Text);


            Cursor = Cursors.WaitCursor; // clock cursor
            ImageClass.Scale_point_xy(img, imgCopy, (float)scaleFactor, x, y);

            ImageViewer.Image = img;
            ImageViewer.Refresh(); // refresh image on the screen

            Cursor = Cursors.Default; // normal cursor 
        }

        private void mediaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (img == null) // verify if the image is already opened
                return;

            //copy Undo Image
            imgUndo = img.Copy();
            Image<Bgr, Byte> imgCopy = img.Copy();

            Cursor = Cursors.WaitCursor; // clock cursor
            ImageClass.Mean(img, imgCopy);

            ImageViewer.Image = img;
            ImageViewer.Refresh(); // refresh image on the screen

            Cursor = Cursors.Default; // normal cursor 
        }

        private void nonUniformToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (img == null) // verify if the image is already opened
                return;

            //copy Undo Image
            imgUndo = img.Copy();
            Image<Bgr, Byte> imgCopy = img.Copy();

            FilterForm ff = new FilterForm();
            if (ff.ShowDialog() == DialogResult.OK)
            {
                float[,] matrix = ff.matrix3;
                float weight = ff.weight;
                float offset = ff.offset;   
                Cursor = Cursors.WaitCursor; // clock cursor
                ImageClass.NonUniform(img, imgCopy, matrix, weight, offset);

                ImageViewer.Image = img;
                ImageViewer.Refresh(); // refresh image on the screen

                Cursor = Cursors.Default; // normal cursor 
            }
        }

        private void sobelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (img == null) // verify if the image is already opened
                return;

            //copy Undo Image
            imgUndo = img.Copy();
            Image<Bgr, Byte> imgCopy = img.Copy();
            Cursor = Cursors.WaitCursor; // clock cursor
            ImageClass.Sobel(img, imgCopy);

            ImageViewer.Image = img;
            ImageViewer.Refresh(); // refresh image on the screen

            Cursor = Cursors.Default; // normal cursor 
        }

        private void medianToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (img == null) // verify if the image is already opened
                return;

            //copy Undo Image
            imgUndo = img.Copy();
            Image<Bgr, Byte> imgCopy = img.Copy();
            Cursor = Cursors.WaitCursor; // clock cursor
            ImageClass.Median(img, imgCopy);

            ImageViewer.Image = img;
            ImageViewer.Refresh(); // refresh image on the screen

            Cursor = Cursors.Default; // normal cursor 
        }

        private void histogramaToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (img == null) // verify if the image is already opened
                return;

            int[] hist_arr = new int[256];
            Cursor = Cursors.WaitCursor; // clock cursor
            hist_arr = ImageClass.Histogram_Gray(img);

            Histograma his = new Histograma(hist_arr);
            his.ShowDialog();
            Cursor = Cursors.Default; // normal cursor 
        }

        private void bWSimpleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (img == null) // verify if the image is already opened
                return;

            int threshold = 124;
            Cursor = Cursors.WaitCursor; // clock cursor
            ImageClass.ConvertToBW(img, threshold);

            ImageViewer.Image = img;
            ImageViewer.Refresh(); // refresh image on the screen

            Cursor = Cursors.Default; // normal cursor 
        }

        private void BW_OTSUToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (img == null) // verify if the image is already opened
                return;

            Cursor = Cursors.WaitCursor; // clock cursor
            ImageClass.ConvertToBW_Otsu(img);

            ImageViewer.Image = img;
            ImageViewer.Refresh(); // refresh image on the screen

            Cursor = Cursors.Default; // normal cursor 
        }

        private void signalIdentifierToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (img == null) // verify if the image is already opened
                return;

            //copy Undo Image
            imgUndo = img.Copy();
            Image<Bgr, Byte> imgCopy = img.Copy();

            Results sinalResult = new Results();

            Cursor = Cursors.WaitCursor; // clock cursor
            ImageClass.SinalReader(img, imgCopy, 0, out sinalResult);

            ImageViewer.Image = img;
            ImageViewer.Refresh(); // refresh image on the screen

            Cursor = Cursors.Default; // normal cursor 
        }
    }
}