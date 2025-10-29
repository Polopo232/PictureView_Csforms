using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace PictureView
{
    public partial class Match : Form
    {

        private TableLayoutPanel tableLayoutPanel1;
        private Label[] labels;
        private Timer timer1;

        public Match()
        {
            // Настройки формы
            this.Text = "Matching Game";
            this.ClientSize = new Size(534, 511);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Создаём TableLayoutPanel
            tableLayoutPanel1 = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                BackColor = SystemColors.Highlight,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.Inset,
                ColumnCount = 4,
                RowCount = 4
            };

            // Добавляем равные пропорции столбцов и строк
            for (int i = 0; i < 4; i++)
            {
                tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
                tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            }

            // Создаём 16 меток
            labels = new Label[16];
            int index = 0;

            for (int row = 0; row < 4; row++)
            {
                for (int col = 0; col < 4; col++)
                {
                    Label lbl = new Label
                    {
                        Dock = DockStyle.Fill,
                        Font = new Font("Microsoft Sans Serif", 48F),
                        Text = "C",
                        TextAlign = ContentAlignment.MiddleCenter,
                        Name = "label" + (index + 1),
                    };

                    lbl.Click += label1_Click;

                    labels[index] = lbl;
                    tableLayoutPanel1.Controls.Add(lbl, col, row);
                    index++;
                }
            }

            // Таймер
            timer1 = new Timer();
            timer1.Tick += timer1_Tick;

            // Добавляем всё на форму
            Controls.Add(tableLayoutPanel1);

            AssignIconsToSquares();
        }

        Label firstClicked = null;

        Label secondClicked = null;


        Random random = new Random();

        List<string> icons = new List<string>()
    {
        "❀", "❀", "♙", "♙", "♫", "♫", "♡", "♡",
        "♢", "♢", "↯", "↯", "★", "★", "℻", "℻"
    };

        private void AssignIconsToSquares()
        {
            foreach (Control control in tableLayoutPanel1.Controls)
            {
                Label iconLabel = control as Label;
                if (iconLabel != null)
                {
                    int randomNumber = random.Next(icons.Count);
                    iconLabel.Text = icons[randomNumber];
                    iconLabel.ForeColor = iconLabel.BackColor;
                    icons.RemoveAt(randomNumber);
                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            if (timer1.Enabled == true)
                return;

            Label clickedLabel = sender as Label;

            if (clickedLabel != null)
            {
                if (clickedLabel.ForeColor == Color.Black)
                    return;

                if (firstClicked == null)
                {
                    firstClicked = clickedLabel;
                    firstClicked.ForeColor = Color.Black;
                    return;
                }

                secondClicked = clickedLabel;
                secondClicked.ForeColor = Color.Black;

                CheckForWinner();

                if (firstClicked.Text == secondClicked.Text)
                {
                    firstClicked = null;
                    secondClicked = null;
                    return;
                }

                timer1.Start();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();

            firstClicked.ForeColor = firstClicked.BackColor;
            secondClicked.ForeColor = secondClicked.BackColor;

            firstClicked = null;
            secondClicked = null;
        }

        private void CheckForWinner()
        {
            foreach (Control control in tableLayoutPanel1.Controls)
            {
                Label iconLabel = control as Label;

                if (iconLabel != null)
                {
                    if (iconLabel.ForeColor == iconLabel.BackColor)
                        return;
                }
            }

            MessageBox.Show("You matched all the icons!", "Congratulations");
            Close();
        }
    }
}
