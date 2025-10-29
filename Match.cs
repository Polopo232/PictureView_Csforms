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

        private int GridSize;
        private int movesCount = 0;

        public Match()
        {
            this.Text = "Matching Game";
            this.ClientSize = new Size(534, 511);
            this.StartPosition = FormStartPosition.CenterScreen;

            DialogResult difficultyChoice = MessageBox.Show(
                "Valige raskusaste:\nYes = Lihtne\nNo = Keskmine\nCancel = Raske",
                "Raskusastme valik",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Question
            );

            if (difficultyChoice == DialogResult.Yes) GridSize = 4;
            else if (difficultyChoice == DialogResult.No) GridSize = 6;
            else GridSize = 10;

            tableLayoutPanel1 = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                BackColor = SystemColors.Highlight,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.Inset,
                ColumnCount = GridSize,
                RowCount = GridSize
            };

            for (int i = 0; i < GridSize; i++)
            {
                tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / GridSize));
                tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100f / GridSize));
            }

            labels = new Label[GridSize * GridSize];
            int index = 0;

            for (int row = 0; row < GridSize; row++)
            {
                for (int col = 0; col < GridSize; col++)
                {
                    Label lbl = new Label
                    {
                        Dock = DockStyle.Fill,
                        Font = new Font("Microsoft Sans Serif", 48F - (GridSize * 2)),
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

            timer1 = new Timer();
            timer1.Interval = 750;
            timer1.Tick += timer1_Tick;

            Controls.Add(tableLayoutPanel1);

            AssignIconsToSquares();
        }

        Label firstClicked = null;
        Label secondClicked = null;

        Random random = new Random();
        List<string> icons = new List<string>()
        {
            "❀","❀","♙","♙","♫","♫","♡","♡",
            "♢","♢","↯","↯","★","★","℻","℻",
            "♤","♤","♧","♧","☀","☀","☁","☁",
            "☂","☂","☃","☃","☯","☯","☮","☮",
            "☢","☢","☣","☣","☠","☠","⚑","⚑",
            "⚡","⚡","⚜","⚜","⚙","⚙","⚛","⚛",
            "⚔","⚔","⚖","⚖","⚗","⚗","⚕","⚕",
            "⚘","⚘","⚙","⚙","⚚","⚚","⚛","⚛",
            "⚜","⚜","⚝","⚝","⚞","⚞","⚟","⚟",
            "⚠","⚠","⚡","⚡","⚢","⚢","⚣","⚣",
            "⚤","⚤","⚥","⚥","⚦","⚦","⚧","⚧",
            "⚨","⚨","⚩","⚩","⚪","⚪","⚫","⚫",
            "⚬","⚬","⚭","⚭","⚮","⚮","⚯","⚯",
            "⚰","⚰","⚱","⚱","⚲","⚲","⚳","⚳",
            "⚴","⚴","⚵","⚵","⚶","⚶","⚷","⚷",
            "⚸","⚸","⚹","⚹","⚺","⚺","⚻","⚻",
            "⚼","⚼","⚽","⚽","⚾","⚾"
        };

        private void AssignIconsToSquares()
        {
            List<string> iconsCopy = new List<string>(icons);
            int totalIconsNeeded = GridSize * GridSize;

            List<string> chosen = new List<string>();
            while (chosen.Count < totalIconsNeeded / 2 && iconsCopy.Count > 0)
            {
                string icon = iconsCopy[random.Next(iconsCopy.Count)];
                iconsCopy.Remove(icon);
                chosen.Add(icon);
            }

            List<string> gameIcons = new List<string>();
            foreach (string icon in chosen)
            {
                gameIcons.Add(icon);
                gameIcons.Add(icon);
            }

            Shuffle(gameIcons);

            int i = 0;
            foreach (Control control in tableLayoutPanel1.Controls)
            {
                Label iconLabel = control as Label;
                if (iconLabel != null)
                {
                    iconLabel.Text = gameIcons[i];
                    iconLabel.ForeColor = iconLabel.BackColor;
                    i++;
                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            if (timer1.Enabled) return;

            Label clickedLabel = sender as Label;
            if (clickedLabel == null) return;

            if (clickedLabel.ForeColor == Color.Black)
                return;

            if (firstClicked == null)
            {
                firstClicked = clickedLabel;
                firstClicked.ForeColor = Color.Black;
                return;
            }

            movesCount++;

            secondClicked = clickedLabel;
            secondClicked.ForeColor = Color.Black;

            CheckForWinner();

            if (firstClicked != null && secondClicked != null && firstClicked.Text == secondClicked.Text)
            {
                firstClicked = null;
                secondClicked = null;
                return;
            }

            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();

            if (firstClicked == null) return;

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

            DialogResult result = MessageBox.Show(
                "You matched all the icons!\n" +
                $"You made {movesCount} turns.\n\nDo you want to play again?",
                "Congratulations",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Information
            );

            if (result == DialogResult.Yes)
            {
                RestartGame();
            }
            else
            {
                Close();
            }
        }

        private void RestartGame()
        {
            tableLayoutPanel1.Controls.Clear();
            movesCount = 0;
            firstClicked = null;
            secondClicked = null;

            labels = new Label[GridSize * GridSize];
            int index = 0;

            for (int row = 0; row < GridSize; row++)
            {
                for (int col = 0; col < GridSize; col++)
                {
                    Label lbl = new Label
                    {
                        Dock = DockStyle.Fill,
                        Font = new Font("Microsoft Sans Serif", 48F - (GridSize * 2)),
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

            AssignIconsToSquares();
        }


        private void Shuffle<T>(List<T> list)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                (list[i], list[j]) = (list[j], list[i]);
            }
        }
    }
}
