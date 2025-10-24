using System;
using System.Drawing;
using System.Windows.Forms;

namespace PictureView
{
    public partial class Math : Form
    {
        Random randomizer = new Random();

        int addend1, addend2, minuend, subtrahend;
        int multiplicand, multiplier, dividend, divisor;
        int timeLeft;
        int bonusTime;

        Label timeLabel, label1;
        Label plusLeftLabel, plusRightLabel, label2, label3;
        Label minusLeftLabel, minusRightLabel, label5, minus;
        Label timesLeftLabel, timesRightLabel, label9, times;
        Label dividedLeftLabel, dividedRightLabel, label13, quotient;
        NumericUpDown sum, difference, product, quotientBox;
        Button startButton;
        Timer timer;

        public Math()
        {
            this.Text = "Matemaatika viktoriin";
            this.ClientSize = new Size(490, 400);
            this.FormBorderStyle = FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            InitializeComponents();
        }

        private void InitializeComponents()
        {
            // Таймер
            label1 = new Label()
            {
                Text = "Time Left",
                Font = new Font("Microsoft Sans Serif", 15.75F),
                Location = new Point(165, 14),
                AutoSize = true
            };
            timeLabel = new Label()
            {
                BorderStyle = BorderStyle.FixedSingle,
                Location = new Point(272, 9),
                Size = new Size(200, 30),
                TextAlign = ContentAlignment.MiddleCenter
            };
            Controls.Add(label1);
            Controls.Add(timeLabel);

            // +
            plusLeftLabel = MakeLabel("?", 50, 70);
            plusRightLabel = MakeLabel("?", 182, 70);
            label2 = MakeLabel("+", 116, 70);
            label3 = MakeLabel("=", 248, 70);
            sum = MakeNumeric(314, 79);

            // -
            minusLeftLabel = MakeLabel("?", 50, 120);
            minusRightLabel = MakeLabel("?", 182, 120);
            minus = MakeLabel("-", 116, 120);
            label5 = MakeLabel("=", 248, 120);
            difference = MakeNumeric(314, 129);

            // *
            timesLeftLabel = MakeLabel("?", 50, 170);
            timesRightLabel = MakeLabel("?", 182, 170);
            times = MakeLabel("*", 116, 170);
            label9 = MakeLabel("=", 248, 170);
            product = MakeNumeric(314, 179);

            // /
            dividedLeftLabel = MakeLabel("?", 50, 220);
            dividedRightLabel = MakeLabel("?", 182, 220);
            quotient = MakeLabel("÷", 116, 220);
            label13 = MakeLabel("=", 248, 220);
            quotientBox = MakeNumeric(314, 229);

            // start button
            startButton = new Button()
            {
                Text = "Alusta viktoriini",
                Font = new Font("Microsoft Sans Serif", 14F),
                Location = new Point(187, 304),
                AutoSize = true
            };
            startButton.Click += startButton_Click;

            Controls.AddRange(new Control[]
            {
                plusLeftLabel, plusRightLabel, label2, label3, sum,
                minusLeftLabel, minusRightLabel, minus, label5, difference,
                timesLeftLabel, timesRightLabel, times, label9, product,
                dividedLeftLabel, dividedRightLabel, quotient, label13, quotientBox,
                startButton
            });

            timer = new Timer();
            timer.Interval = 1000;
            timer.Tick += TimerEvent;

            // Подписка на изменения ответов
            sum.ValueChanged += AnswerChanged;
            difference.ValueChanged += AnswerChanged;
            product.ValueChanged += AnswerChanged;
            quotientBox.ValueChanged += AnswerChanged;
        }

        private Label MakeLabel(string text, int x, int y)
        {
            return new Label()
            {
                Text = text,
                Font = new Font("Microsoft Sans Serif", 18F),
                Location = new Point(x, y),
                Size = new Size(60, 50),
                TextAlign = ContentAlignment.MiddleCenter
            };
        }

        private NumericUpDown MakeNumeric(int x, int y)
        {
            return new NumericUpDown()
            {
                Font = new Font("Microsoft Sans Serif", 18F),
                Location = new Point(x, y),
                Size = new Size(120, 35),
                Minimum = 0,
                Maximum = 1000
            };
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            DialogResult difficultyChoice = MessageBox.Show(
                "Valige raskusaste:\nYes = Lihtne\nNo = Keskmine\nCancel = Raske",
                "Raskusastme valik",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Question
            );

            int maxValue;

            if (difficultyChoice == DialogResult.Yes) { maxValue = 11; bonusTime = 1; }
            else if (difficultyChoice == DialogResult.No) { maxValue = 51; bonusTime = 5; }
            else { maxValue = 101; bonusTime = 10; }

            ResetHighlights();
            StartTheQuiz(maxValue);
        }

        public void StartTheQuiz(int max)
        {
            addend1 = randomizer.Next(max);
            addend2 = randomizer.Next(max);
            plusLeftLabel.Text = addend1.ToString();
            plusRightLabel.Text = addend2.ToString();
            sum.Value = 0;

            minuend = randomizer.Next(1, max * 2);
            subtrahend = randomizer.Next(1, minuend);
            minusLeftLabel.Text = minuend.ToString();
            minusRightLabel.Text = subtrahend.ToString();
            difference.Value = 0;

            multiplicand = randomizer.Next(2, System.Math.Min(11, max));
            multiplier = randomizer.Next(2, System.Math.Min(11, max));
            timesLeftLabel.Text = multiplicand.ToString();
            timesRightLabel.Text = multiplier.ToString();
            product.Value = 0;

            divisor = randomizer.Next(2, System.Math.Min(11, max));
            int temporaryQuotient = randomizer.Next(2, System.Math.Min(11, max));
            dividend = divisor * temporaryQuotient;
            dividedLeftLabel.Text = dividend.ToString();
            dividedRightLabel.Text = divisor.ToString();
            quotientBox.Value = 0;

            timeLeft = 30;
            timeLabel.Text = "30 sekundid";
            timer.Start();
            startButton.Enabled = false;
        }

        private void TimerEvent(object sender, EventArgs e)
        {
            if (CheckAnswers())
            {
                timer.Stop();
                HighlightAnswers();
                MessageBox.Show("Sa vastasid kõikidele küsimustele õigesti!", "Õnnitlused!");
                startButton.Enabled = true;
            }
            else if (timeLeft > 0)
            {
                timeLeft--;
                timeLabel.Text = timeLeft + " sekundid";
            }
            else
            {
                timer.Stop();
                timeLabel.Text = "Aeg on otsas!";
                HighlightAnswers();
                ShowAnswers();
                MessageBox.Show("Sa ei jõudnud õigeks ajaks valmis.", "Vabandust!");
                startButton.Enabled = true;
            }
        }

        private void AnswerChanged(object sender, EventArgs e)
        {
            NumericUpDown box = sender as NumericUpDown;

            if (box.Value == 0)
            {
                box.BackColor = SystemColors.Window;
                return;
            }
            if (box == sum)
                box.BackColor = (addend1 + addend2 == sum.Value) ? Color.LightGreen : Color.LightCoral;
            else if (box == difference)
                box.BackColor = (minuend - subtrahend == difference.Value) ? Color.LightGreen : Color.LightCoral;
            else if (box == product)
                box.BackColor = (multiplicand * multiplier == product.Value) ? Color.LightGreen : Color.LightCoral;
            else if (box == quotientBox)
                box.BackColor = (dividend / divisor == quotientBox.Value) ? Color.LightGreen : Color.LightCoral;

            if ((box == sum && addend1 + addend2 == sum.Value) ||
                (box == difference && minuend - subtrahend == difference.Value) ||
                (box == product && multiplicand * multiplier == product.Value) ||
                (box == quotientBox && dividend / divisor == quotientBox.Value))
            {
                timeLeft += bonusTime;
                timeLabel.Text = timeLeft + " sekundid";
            }

            if (CheckAnswers())
            {
                timer.Stop();
                MessageBox.Show("Sa vastasid kõikidele küsimustele õigesti!", "Õnnitlused!");
                startButton.Enabled = true;
            }
        }

        private bool CheckAnswers()
        {
            return (addend1 + addend2 == sum.Value)
                && (minuend - subtrahend == difference.Value)
                && (multiplicand * multiplier == product.Value)
                && (dividend / divisor == quotientBox.Value);
        }

        private void ShowAnswers()
        {
            sum.Value = addend1 + addend2;
            difference.Value = minuend - subtrahend;
            product.Value = multiplicand * multiplier;
            quotientBox.Value = dividend / divisor;
        }

        private void HighlightAnswers()
        {
            sum.BackColor = (addend1 + addend2 == sum.Value) ? Color.LightGreen : Color.LightCoral;
            difference.BackColor = (minuend - subtrahend == difference.Value) ? Color.LightGreen : Color.LightCoral;
            product.BackColor = (multiplicand * multiplier == product.Value) ? Color.LightGreen : Color.LightCoral;
            quotientBox.BackColor = (dividend / divisor == quotientBox.Value) ? Color.LightGreen : Color.LightCoral;
        }

        private void ResetHighlights()
        {
            sum.BackColor = SystemColors.Window;
            sum.Value = 0;

            difference.BackColor = SystemColors.Window;
            difference.Value = 0;

            product.BackColor = SystemColors.Window;
            product.Value = 0;

            quotientBox.BackColor = SystemColors.Window;
            quotientBox.Value = 0;
        }
    }
}
