using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace SS_OpenCV
{
    public partial class Histograma : Form
    {

        public Histograma(int[] array)
        {
            InitializeComponent();
            DataPointCollection list1 = chart1.Series[0].Points;

            for (int i = 0; i < array.Length; ++i)
            {
                list1.AddXY(i, array[i]);
            }

            chart1.Series[0].Color = Color.Gray;
            chart1.ChartAreas[0].AxisX.Maximum = 255;
            chart1.ChartAreas[0].AxisX.Minimum = 0;
            chart1.ChartAreas[0].AxisX.Title = "Intensidade";
            chart1.ChartAreas[0].AxisY.Title = "Numero Pixeis";

            chart1.Series.RemoveAt(3);
            chart1.Series.RemoveAt(2);
            chart1.Series.RemoveAt(1);

            chart1.ResumeLayout();
        }

        public Histograma(int[,] array, int to_show)
        {
            int rows = array.GetLength(0);
            int cols = array.GetLength(1);
            InitializeComponent();
            DataPointCollection[] lists = new DataPointCollection[rows];

            for (int i = 0; i < rows; ++i)
            {
                lists[i] = chart1.Series[i].Points;
                for (int j = 0; j < cols; ++j)
                {
                    lists[i].AddXY(j, array[i, j]);
                }
            }

            chart1.ChartAreas[0].AxisX.Maximum = 255;
            chart1.ChartAreas[0].AxisX.Minimum = 0;
            chart1.ChartAreas[0].AxisX.Title = "Intensidade";
            chart1.ChartAreas[0].AxisY.Title = "Numero Pixeis";

            for (int i = rows - 1; i > to_show - 1; --i)
            {
                chart1.Series.RemoveAt(i);
            }

            if (to_show == 3) {
                chart1.Series[0].Color = Color.Blue;
                chart1.Series[1].Color = Color.Green;
                chart1.Series[2].Color = Color.Red;
            } else if (to_show == 4) {
                chart1.Series[0].Color = Color.Gray; 
                chart1.Series[1].Color = Color.Blue;
                chart1.Series[2].Color = Color.Green;
                chart1.Series[3].Color = Color.Red;
            }
            
            chart1.ResumeLayout();
        }
    }
}
