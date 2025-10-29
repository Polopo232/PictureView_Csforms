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
            this.ClientSize = new Size(600, 550);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Выбор сложности
            DialogResult difficultyChoice = MessageBox.Show(
                "Valige raskusaste:\nYes = Lihtne\nNo = Keskmine\nCancel = Raske",
                "Raskusastme valik",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Question
            );

            if (difficultyChoice == DialogResult.Yes) GridSize = 4;
            else if (difficultyChoice == DialogResult.No) GridSize = 6;
            else GridSize = 10;

            TabControl tabControl = new TabControl { Dock = DockStyle.Fill };
            TabPage gameTab = new TabPage("Game");
            TabPage settingsTab = new TabPage("Settings");

            tabControl.TabPages.Add(gameTab);
            tabControl.TabPages.Add(settingsTab);
            this.Controls.Add(tabControl);

            // Таблица игры
            tableLayoutPanel1 = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                BackColor = Properties.Settings.Default.PanelColor != Color.Empty
                            ? Properties.Settings.Default.PanelColor
                            : SystemColors.Highlight,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.Inset,
                ColumnCount = GridSize,
                RowCount = GridSize
            };

            for (int i = 0; i < GridSize; i++)
            {
                tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / GridSize));
                tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100f / GridSize));
            }

            gameTab.Controls.Add(tableLayoutPanel1);

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
                        ForeColor = tableLayoutPanel1.BackColor
                    };
                    lbl.Click += label1_Click;
                    labels[index] = lbl;
                    tableLayoutPanel1.Controls.Add(lbl, col, row);
                    index++;
                }
            }

            timer1 = new Timer { Interval = 750 };
            timer1.Tick += timer1_Tick;

            AssignIconsToSquares();

            // Настройки
            FlowLayoutPanel settingsPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                AutoSize = true,
                Padding = new Padding(10),
                Margin = new Padding(0, 20, 0, 0)
            };
            settingsTab.Controls.Add(settingsPanel);

            Label lblSettings = new Label
            {
                Text = "Choose panel color:",
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleLeft,
                Margin = new Padding(0, 0, 0, 10)
            };
            settingsPanel.Controls.Add(lblSettings);

            Button btnRed = new Button { Text = "Red", Width = 80, Height = 30, Margin = new Padding(5) };
            Button btnGreen = new Button { Text = "Green", Width = 80, Height = 30, Margin = new Padding(5) };
            Button btnBlue = new Button { Text = "Blue", Width = 80, Height = 30, Margin = new Padding(5) };
            Button btnCustom = new Button { Text = "Custom...", Width = 100, Height = 30, Margin = new Padding(5) };

            btnRed.Click += (s, e) => ChangePanelColor(Color.Red);
            btnGreen.Click += (s, e) => ChangePanelColor(Color.Green);
            btnBlue.Click += (s, e) => ChangePanelColor(Color.Blue);
            btnCustom.Click += (s, e) =>
            {
                using (ColorDialog cd = new ColorDialog())
                {
                    if (cd.ShowDialog() == DialogResult.OK)
                        ChangePanelColor(cd.Color);
                }
            };

            settingsPanel.Controls.Add(btnRed);
            settingsPanel.Controls.Add(btnGreen);
            settingsPanel.Controls.Add(btnBlue);
            settingsPanel.Controls.Add(btnCustom);
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

        private void ChangePanelColor(Color color)
        {
            tableLayoutPanel1.BackColor = color;

            bool isDark = color.GetBrightness() < 0.5f;
            Color textColor = isDark ? Color.White : Color.Black;

            foreach (Label lbl in labels)
            {
                if (lbl.ForeColor != Color.Black)
                    lbl.ForeColor = textColor;
            }

            Properties.Settings.Default.PanelColor = color;
            Properties.Settings.Default.Save();
        }
    }
}
