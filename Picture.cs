using System;
using System.Drawing;
using System.Windows.Forms;

namespace PictureView
{
    public class Picture : Form
    {
        private float zoomFactor = 1.0f;
        private bool isDrawing = false;
        private Point lastPoint;
        private bool drawingEnabled = false;
        private Color penColor = Color.Red;
        private int penWidth = 2;

        Panel picturePanel;
        PictureBox pictureBox;
        CheckBox stretchCheckBox, drawCheckBox;
        Button showButton, clearButton, backgroundButton, closeButton, colorButton;
        Button zoomInButton, zoomOutButton, saveButton;
        OpenFileDialog openFileDialog;
        ColorDialog colorDialog;

        public Picture()
        {
            Text = "Picture Viewer";
            ClientSize = new Size(900, 500);
            FormBorderStyle = FormBorderStyle.Fixed3D;
            MaximizeBox = false;
            StartPosition = FormStartPosition.CenterScreen;

            InitializeComponents();
        }

        private void InitializeComponents()
        {
            // Above panel with buttons and checkboxes
            FlowLayoutPanel topPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 40,
                FlowDirection = FlowDirection.LeftToRight,
                Padding = new Padding(5),
                AutoSize = true
            };

            // Buttons
            showButton = new Button { Text = "Show" };
            clearButton = new Button { Text = "Clear" };
            backgroundButton = new Button { Text = "Background" };
            closeButton = new Button { Text = "Close" };
            zoomInButton = new Button { Text = "Zoom In" };
            zoomOutButton = new Button { Text = "Zoom Out" };
            saveButton = new Button { Text = "Save" };
            colorButton = new Button { Text = "Pen Color" };

            // Checkboxes
            stretchCheckBox = new CheckBox { Text = "Stretch", AutoSize = true };
            drawCheckBox = new CheckBox { Text = "Draw", AutoSize = true };

            // Size buttons uniformly
            int buttonWidth = 80, buttonHeight = 30;
            foreach (Button b in new[] { showButton, clearButton, backgroundButton, closeButton, zoomInButton, zoomOutButton, saveButton, colorButton })
                b.Size = new Size(buttonWidth, buttonHeight);

            // Add a panel
            topPanel.Controls.Add(zoomInButton);
            topPanel.Controls.Add(zoomOutButton);
            topPanel.Controls.Add(showButton);
            topPanel.Controls.Add(clearButton);
            topPanel.Controls.Add(saveButton);
            topPanel.Controls.Add(backgroundButton);
            topPanel.Controls.Add(colorButton);
            topPanel.Controls.Add(stretchCheckBox);
            topPanel.Controls.Add(drawCheckBox);

            Controls.Add(topPanel);

            // Panel for PictureBox
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
            Controls.Add(picturePanel);

            openFileDialog = new OpenFileDialog
            {
                Title = "Select a picture",
                Filter = "JPEG Files (*.jpg)|*.jpg|PNG Files (*.png)|*.png|BMP Files (*.bmp)|*.bmp|All files (*.*)|*.*"
            };
            colorDialog = new ColorDialog();

            showButton.Click += ShowButton_Click;
            clearButton.Click += (s, e) => pictureBox.Image = null;
            backgroundButton.Click += BackgroundButton_Click;
            closeButton.Click += (s, e) => Close();
            zoomInButton.Click += ZoomInButton_Click;
            zoomOutButton.Click += ZoomOutButton_Click;
            saveButton.Click += SaveButton_Click;
            colorButton.Click += ColorButton_Click;
            stretchCheckBox.CheckedChanged += StretchCheckBox_CheckedChanged;
            drawCheckBox.CheckedChanged += DrawCheckBox_CheckedChanged;

            // Draw by mouse events
            pictureBox.MouseDown += PictureBox_MouseDown;
            pictureBox.MouseMove += PictureBox_MouseMove;
            pictureBox.MouseUp += PictureBox_MouseUp;
        }

        private void DrawCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            drawingEnabled = drawCheckBox.Checked;
            pictureBox.Cursor = drawingEnabled ? Cursors.Cross : Cursors.Default;
        }

        private void StretchCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (stretchCheckBox.Checked)
            {
                pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            else
            {
                pictureBox.SizeMode = PictureBoxSizeMode.Normal;
            }
            ApplyZoom();
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

            if (stretchCheckBox.Checked)
            {
                pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;

                int newWidth = (int)(pictureBox.Image.Width * zoomFactor);
                int newHeight = (int)(pictureBox.Image.Height * zoomFactor);

                Size panelSize = picturePanel.ClientSize;
                float scaleX = (float)panelSize.Width / newWidth;
                float scaleY = (float)panelSize.Height / newHeight;
                float scale = System.Math.Min(scaleX, scaleY);

                int displayWidth = (int)(newWidth * scaleX);
                int displayHeight = (int)(newHeight * scaleY);

                pictureBox.Size = panelSize;
            }
            else
            {
                pictureBox.SizeMode = PictureBoxSizeMode.Normal;
                int w = (int)(pictureBox.Image.Width * zoomFactor);
                int h = (int)(pictureBox.Image.Height * zoomFactor);
                pictureBox.Size = new Size(w, h);
            }

            pictureBox.Invalidate();
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (pictureBox.Image == null) return;

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Title = "Save picture";
                sfd.Filter = "PNG Image|*.png|JPEG Image|*.jpg|Bitmap Image|*.bmp";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    pictureBox.Image.Save(sfd.FileName);
                    MessageBox.Show("Image saved!", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void ColorButton_Click(object sender, EventArgs e)
        {
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                penColor = colorDialog.Color;
            }
        }

        private void PictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (!drawingEnabled || pictureBox.Image == null) return;
            isDrawing = true;
            lastPoint = e.Location;
        }

        private void PictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (!isDrawing || pictureBox.Image == null) return;

            using (Graphics g = Graphics.FromImage(pictureBox.Image))
            {
                Pen pen = new Pen(penColor, penWidth);
                g.DrawLine(pen, lastPoint, e.Location);
            }
            pictureBox.Invalidate();
            lastPoint = e.Location;
        }

        private void PictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            isDrawing = false;
        }
    }
}
