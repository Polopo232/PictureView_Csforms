using System;
using System.Windows.Forms;

namespace PictureView
{
    public partial class Picture : Form
    {
        public Picture()
        {
            InitializeComponent();
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tableLayoutPanel3_Paint(object sender, PaintEventArgs e)
        {

        }

        //Show picture button

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            else
                pictureBox1.SizeMode = PictureBoxSizeMode.Normal;
        }

        //Close button
        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //Show background button
        private void showBackButton_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
                pictureBox1.BackColor = colorDialog1.Color;
        }

        //Clear picture button
        private void clearButton_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = null;
        }

        //Show a picture button
        private void showButton_Click_1(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Load(openFileDialog1.FileName);

            }
        }
    }
}
