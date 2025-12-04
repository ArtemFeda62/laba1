using System;
using System.Drawing;
using System.Reflection.Emit;
using System.Windows.Forms;

namespace View.Forms
{
    public class HitHeroForm : Form
    {
        public double DamageAmount { get; private set; }

        private NumericUpDown numDamage;
        private Button btnOk, btnCancel;

        public DamageHeroForm(string heroName, double currentHp)
        {
            InitializeComponent(heroName, currentHp);
        }

        private void InitializeComponent(string heroName, double currentHp)
        {
            this.Text = $"Нанести урон герою {heroName}";
            this.Size = new Size(300, 150);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;

            var lblInfo = new Label
            {
                Text = $"Герой: {heroName}\nТекущее HP: {currentHp:F1}\n\nВведите урон:",
                Location = new Point(10, 10),
                Width = 280,
                Height = 60
            };

            numDamage = new NumericUpDown
            {
                Location = new Point(10, 75),
                Width = 150,
                Minimum = 0.1M,
                Maximum = (decimal)currentHp * 2,
                Value = (decimal)Math.Min(10, currentHp),
                DecimalPlaces = 1,
                Increment = 0.1M
            };

            btnOk = new Button { Text = "Нанести урон", Location = new Point(50, 105), Width = 100, DialogResult = DialogResult.OK };
            btnCancel = new Button { Text = "Отмена", Location = new Point(160, 105), Width = 80, DialogResult = DialogResult.Cancel };

            btnOk.Click += (s, e) =>
            {
                if (numDamage.Value <= 0)
                {
                    MessageBox.Show("Урон должен быть больше 0", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                DamageAmount = (double)numDamage.Value;
                DialogResult = DialogResult.OK;
                Close();
            };

            this.Controls.AddRange(new Control[] { lblInfo, numDamage, btnOk, btnCancel });
        }
    }
}