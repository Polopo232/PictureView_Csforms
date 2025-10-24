using System;
using System.Drawing;
using System.Windows.Forms;

namespace PictureView
{
    public class Picture : Form
    {
        TableLayoutPanel mainLayout;
        PictureBox pictureBox;
        CheckBox stretchCheckBox;
        FlowLayoutPanel buttonPanel;
        Button showButton, clearButton, backgroundButton, closeButton;
        OpenFileDialog openFileDialog;
        ColorDialog colorDialog;

        public Picture()
        {
            Text = "Picture Viewer";
            ClientSize = new Size(800, 500);
            FormBorderStyle = FormBorderStyle.Fixed3D;
            MaximizeBox = false;
            StartPosition = FormStartPosition.CenterScreen;

            InitializeComponents();
        }

        private void InitializeComponents()
        {
            // main
            mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 2
            };
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15F));
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 85F));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 90F));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));

            // PictureBox
            pictureBox = new PictureBox
            {
                BorderStyle = BorderStyle.Fixed3D,
                Dock = DockStyle.Fill,
                SizeMode = PictureBoxSizeMode.Normal
            };
            mainLayout.SetColumnSpan(pictureBox, 2);
            mainLayout.Controls.Add(pictureBox, 0, 0);

            // CheckBox
            stretchCheckBox = new CheckBox
            {
                Text = "Stretch",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };
            stretchCheckBox.CheckedChanged += StretchCheckBox_CheckedChanged;
            mainLayout.Controls.Add(stretchCheckBox, 0, 1);

            // Buttons panel
            buttonPanel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.RightToLeft,
                Dock = DockStyle.Fill
            };

            // buttons
            closeButton = new Button { Text = "Close" };
            closeButton.Click += (s, e) => Close();

            backgroundButton = new Button { Text = "Set background color" };
            backgroundButton.Click += BackgroundButton_Click;

            clearButton = new Button { Text = "Clear the picture" };
            clearButton.Click += (s, e) => pictureBox.Image = null;

            showButton = new Button { Text = "Show a picture" };
            showButton.Click += ShowButton_Click;

            // buttons size
            int buttonWidth = 150;
            int buttonHeight = 30;

            foreach (Button b in new[] { showButton, clearButton, backgroundButton, closeButton })
            {
                b.Size = new Size(buttonWidth, buttonHeight);
            }

            buttonPanel.Controls.Add(closeButton);
            buttonPanel.Controls.Add(backgroundButton);
            buttonPanel.Controls.Add(clearButton);
            buttonPanel.Controls.Add(showButton);

            mainLayout.Controls.Add(buttonPanel, 1, 1);

            openFileDialog = new OpenFileDialog
            {
                Title = "Select a picture",
                Filter = "JPEG Files (*.jpg)|*.jpg|PNG Files (*.png)|*.png|BMP Files (*.bmp)|*.bmp|All files (*.*)|*.*"
            };

            colorDialog = new ColorDialog();

            Controls.Add(mainLayout);
        }
        private void StretchCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            pictureBox.SizeMode = stretchCheckBox.Checked
                ? PictureBoxSizeMode.StretchImage
                : PictureBoxSizeMode.Normal;
        }

        private void ShowButton_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                pictureBox.Load(openFileDialog.FileName);
            }
        }

        private void BackgroundButton_Click(object sender, EventArgs e)
        {
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                pictureBox.BackColor = colorDialog.Color;
            }
        }
    }
}
