using PictureView;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace PictureView
{
    public partial class MainForm : Form
    {
        private Button buttonMath;
        private Button buttonMatch;
        private Button buttonPicture;
        private Label titleLabel;

        public MainForm()
        {
            this.Text = "Peamenüü";
            this.Size = new Size(350, 300);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = Color.WhiteSmoke;

            titleLabel = new Label();
            titleLabel.Text = "Vali programm:";
            titleLabel.AutoSize = true;
            titleLabel.Location = new Point(110, 30);

            buttonPicture = new Button();
            buttonPicture.Text = "Picture";
            buttonPicture.Size = new Size(150, 40);
            buttonPicture.Location = new Point(90, 80);
            buttonPicture.Click += ButtonPicture_Click;

            buttonMath = new Button();
            buttonMath.Text = "Math";
            buttonMath.Size = new Size(150, 40);
            buttonMath.Location = new Point(90, 130);
            buttonMath.Click += ButtonMath_Click;

            buttonMatch = new Button();
            buttonMatch.Text = "Match";
            buttonMatch.Size = new Size(150, 40);
            buttonMatch.Location = new Point(90, 180);
            buttonMatch.Click += ButtonMatch_Click;

            this.Controls.Add(titleLabel);
            this.Controls.Add(buttonPicture);
            this.Controls.Add(buttonMath);
            this.Controls.Add(buttonMatch);
        }

        private void ButtonMath_Click(object sender, EventArgs e)
        {
            Math mathForm = new Math();
            mathForm.Show();
        }

        private void ButtonMatch_Click(object sender, EventArgs e)
        {
            Match matchForm = new Match();
            matchForm.Show();
        }

        private void ButtonPicture_Click(object sender, EventArgs e)
        {
            Picture pictureForm = new Picture();
            pictureForm.Show();
        }
    }
}
