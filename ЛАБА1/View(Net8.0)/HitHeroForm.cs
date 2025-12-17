// View/HitHeroForm.cs
using System;
using System.Drawing;
using System.Windows.Forms;

namespace View
{
    public partial class HitHeroForm : Form
    {
        public double DamageAmount { get; private set; }

        private dynamic _hero;
        private NumericUpDown numDamage;
        private Button btnOk, btnCancel;
        private Label lblCurrentHp;
        private Label lblWarning;

        public HitHeroForm(dynamic hero)
        {
            _hero = hero;
            InitializeForm();
        }

        private void InitializeForm()
        {
            this.Text = $"Нанести урон герою {_hero.Name}";
            this.Size = new Size(350, 200);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;
            this.MaximizeBox = false;

            int y = 10;

            // Информация о герое
            var lblHeroInfo = new Label
            {
                Text = $"Герой: {_hero.Name}",
                Location = new Point(10, y),
                Width = 300,
                Font = new Font(Font, FontStyle.Bold)
            };
            y += 25;

            // Текущее HP
            lblCurrentHp = new Label
            {
                Text = $"Текущее HP: {_hero.Hp:F1}",
                Location = new Point(10, y),
                Width = 150
            };
            y += 25;

            // Урон
            var lblDamage = new Label
            {
                Text = "Урон:",
                Location = new Point(10, y),
                Width = 80
            };
            numDamage = new NumericUpDown
            {
                Location = new Point(90, y - 3),
                Width = 120,
                Minimum = 0,
                Maximum = (decimal)(_hero.Hp * 2),
                Value = (decimal)Math.Min(10, _hero.Hp),
                DecimalPlaces = 1,
                Increment = 5
            };
            y += 35;

            // Предупреждение
            lblWarning = new Label
            {
                Text = "Если урон ≥ текущему HP, герой погибнет!",
                Location = new Point(10, y),
                Width = 300,
                ForeColor = Color.Red,
                Font = new Font(Font, FontStyle.Italic)
            };
            y += 35;

            // Кнопки
            btnOk = new Button
            {
                Text = "Нанести урон",
                Location = new Point(70, y),
                Width = 100
            };
            btnCancel = new Button
            {
                Text = "Отмена",
                Location = new Point(180, y),
                Width = 80
            };

            btnOk.Click += BtnOk_Click;
            btnCancel.Click += (s, e) =>
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            };

            // Подписываемся на изменение значения урона
            numDamage.ValueChanged += NumDamage_ValueChanged;

            this.Controls.AddRange(new Control[] {
                lblHeroInfo,
                lblCurrentHp,
                lblDamage, numDamage,
                lblWarning,
                btnOk, btnCancel
            });

            this.AcceptButton = btnOk;
            this.CancelButton = btnCancel;
        }

        private void NumDamage_ValueChanged(object sender, EventArgs e)
        {
            double damage = (double)numDamage.Value;
            double remainingHp = _hero.Hp - damage;

            if (remainingHp <= 0)
            {
                lblWarning.Text = $"ВНИМАНИЕ: Герой погибнет! (урон: {damage:F1} ≥ HP: {_hero.Hp:F1})";
                lblWarning.ForeColor = Color.DarkRed;
                lblWarning.Font = new Font(lblWarning.Font, FontStyle.Bold);
            }
            else if (remainingHp < 50)
            {
                lblWarning.Text = $"Герой будет ранен. Остаток HP: {remainingHp:F1}";
                lblWarning.ForeColor = Color.Orange;
            }
            else
            {
                lblWarning.Text = $"Остаток HP после удара: {remainingHp:F1}";
                lblWarning.ForeColor = Color.Green;
            }
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            DamageAmount = (double)numDamage.Value;

            if (DamageAmount <= 0)
            {
                MessageBox.Show("Урон должен быть больше 0", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Подтверждение для смертельного урона
            if (DamageAmount >= _hero.Hp)
            {
                var result = MessageBox.Show(
                    $"Вы собираетесь нанести смертельный урон ({DamageAmount:F1}) герою {_hero.Name}.\n" +
                    $"Герой погибнет!\n\nПродолжить?",
                    "Подтверждение смертельного удара",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result != DialogResult.Yes)
                    return;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}