using System;
using System.Drawing;
using System.Windows.Forms;

namespace PictureView
{
    public class Picture : Form
    {
        private float zoomFactor = 1.0f;

        TableLayoutPanel mainLayout;
        Panel picturePanel;
        PictureBox pictureBox;
        CheckBox stretchCheckBox;
        FlowLayoutPanel buttonPanel;
        Button showButton, clearButton, backgroundButton, closeButton;
        Button zoomInButton, zoomOutButton;
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

            picturePanel = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true
            };
            pictureBox = new PictureBox
            {
                BorderStyle = BorderStyle.Fixed3D,
                SizeMode = PictureBoxSizeMode.Zoom
            };
            picturePanel.Controls.Add(pictureBox);
            mainLayout.SetColumnSpan(picturePanel, 2);
            mainLayout.Controls.Add(picturePanel, 0, 0);

            stretchCheckBox = new CheckBox
            {
                Text = "Stretch",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };
            stretchCheckBox.CheckedChanged += StretchCheckBox_CheckedChanged;
            mainLayout.Controls.Add(stretchCheckBox, 0, 1);

            buttonPanel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.RightToLeft,
                Dock = DockStyle.Fill
            };

            showButton = new Button { Text = "Show a picture" };
            showButton.Click += ShowButton_Click;

            clearButton = new Button { Text = "Clear the picture" };
            clearButton.Click += (s, e) => pictureBox.Image = null;

            backgroundButton = new Button { Text = "Set background color" };
            backgroundButton.Click += BackgroundButton_Click;

            closeButton = new Button { Text = "Close" };
            closeButton.Click += (s, e) => Close();

            zoomInButton = new Button { Text = "Zoom In" };
            zoomOutButton = new Button { Text = "Zoom Out" };
            zoomInButton.Click += ZoomInButton_Click;
            zoomOutButton.Click += ZoomOutButton_Click;

            int buttonWidth = 100;
            int buttonHeight = 30;
            foreach (Button b in new[] { showButton, clearButton, backgroundButton, closeButton, zoomInButton, zoomOutButton })
                b.Size = new Size(buttonWidth, buttonHeight);

            buttonPanel.Controls.Add(closeButton);
            buttonPanel.Controls.Add(backgroundButton);
            buttonPanel.Controls.Add(clearButton);
            buttonPanel.Controls.Add(showButton);
            buttonPanel.Controls.Add(zoomOutButton);
            buttonPanel.Controls.Add(zoomInButton);

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
                : PictureBoxSizeMode.Zoom;
        }

        private void ShowButton_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                pictureBox.Image = Image.FromFile(openFileDialog.FileName);
                zoomFactor = 1.0f;
                ApplyZoom();
            }
        }
        private void BackgroundButton_Click(object sender, EventArgs e)
        {
            if (colorDialog.ShowDialog() == DialogResult.OK)
                pictureBox.BackColor = colorDialog.Color;
        }

        private void ZoomInButton_Click(object sender, EventArgs e)
        {
            if (pictureBox.Image == null) return;
            zoomFactor += 0.1f;
            ApplyZoom();
        }

        private void ZoomOutButton_Click(object sender, EventArgs e)
        {
            if (pictureBox.Image == null) return;
            zoomFactor = System.Math.Max(0.1f, zoomFactor - 0.1f);
            ApplyZoom();
        }

        private void ApplyZoom()
        {
            if (pictureBox.Image == null) return;

            pictureBox.Width = (int)(pictureBox.Image.Width * zoomFactor);
            pictureBox.Height = (int)(pictureBox.Image.Height * zoomFactor);
        }
    }
}
