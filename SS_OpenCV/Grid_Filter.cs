using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SS_OpenCV
{
    public partial class Grid_Filter : Form
    {
        public float[,] matrix = new float[3, 3];
        public float weight;
        public float offset;

        public Grid_Filter()
        {
            InitializeComponent();
        }

        private void ok_button_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void cancel_button_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (float.TryParse(textBox1.Text, out float tmp))
                matrix[0, 0] = tmp;
            else
            {
                Console.Write("textBox1 was given an invalid number");
                Environment.Exit(1);
            }
        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (float.TryParse(textBox2.Text, out float tmp))
                matrix[0, 1] = tmp;
            else
            {
                Console.Write("textBox2 was given an invalid number");
                Environment.Exit(1);
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (float.TryParse(textBox3.Text, out float tmp))
                matrix[0, 2] = tmp;
            else
            {
                Console.Write("textBox3 was given an invalid number");
                Environment.Exit(1);
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            if (float.TryParse(textBox4.Text, out float tmp))
                matrix[1, 0] = tmp;
            else
            {
                Console.Write("textBox4 was given an invalid number");
                Environment.Exit(1);
            }
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            if (float.TryParse(textBox5.Text, out float tmp))
                matrix[1, 1] = tmp;
            else
            {
                Console.Write("textBox5 was given an invalid number");
                Environment.Exit(1);
            }
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            if (float.TryParse(textBox6.Text, out float tmp))
                matrix[1, 2] = tmp;
            else
            {
                Console.Write("textBox6 was given an invalid number");
                Environment.Exit(1);
            }
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            if (float.TryParse(textBox7.Text, out float tmp))
                matrix[2, 0] = tmp;
            else
            {
                Console.Write("textBox7 was given an invalid number");
                Environment.Exit(1);
            }
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            if (float.TryParse(textBox8.Text, out float tmp))
                matrix[2, 1] = tmp;
            else
            {
                Console.Write("textBox8 was given an invalid number");
                Environment.Exit(1);
            }
        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {
            if (float.TryParse(textBox9.Text, out float tmp))
                matrix[2, 2] = tmp;
            else
            {
                Console.Write("textBox9 was given an invalid number");
                Environment.Exit(1);
            }
        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {
            if (float.TryParse(textBox9.Text, out float tmp))
                weight = tmp;
            else
            {
                Console.Write("textBox weight was given an invalid number");
                Environment.Exit(1);
            }
        }

        private void textBox11_TextChanged(object sender, EventArgs e)
        {
            if (float.TryParse(textBox9.Text, out float tmp))
                offset = tmp;
            else
            {
                Console.Write("textBox offset was given an invalid number");
                Environment.Exit(1);
            }
        }
    }
}
