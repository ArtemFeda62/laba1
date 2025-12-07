using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public HitHeroForm(dynamic hero)
        {
            _hero = hero;
            InitializeComponent();
            InitializeForm();
        }

        private void InitializeForm()
        {
            this.Text = $"Нанести урон герою {_hero.Name}";
            this.Size = new Size(300, 180);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;
            this.MaximizeBox = false;

            int y = 10;

            // Информация о текущем HP
            lblCurrentHp = new Label
            {
                Text = $"Текущее HP: {_hero.Hp}",
                Location = new Point(10, y),
                Width = 250,
                Font = new Font(Font, FontStyle.Bold)
            };
            y += 30;

            // Урон
            var lblDamage = new Label { Text = "Урон:", Location = new Point(10, y), Width = 80 };
            numDamage = new NumericUpDown
            {
                Location = new Point(90, y - 3),
                Width = 120,
                Minimum = 0,
                Maximum = (decimal)_hero.Hp * 2,
                Value = (decimal)Math.Min(10, _hero.Hp),
                DecimalPlaces = 1
            };
            y += 40;

            // Предупреждение
            var lblWarning = new Label
            {
                Text = "Если урон >= текущему HP, герой умрет",
                Location = new Point(10, y),
                Width = 250,
                ForeColor = Color.Red,
                Font = new Font(Font, FontStyle.Italic)
            };
            y += 40;

            // Кнопки
            btnOk = new Button
            {
                Text = "Нанести урон",
                Location = new Point(50, y),
                Width = 100,
                DialogResult = DialogResult.OK
            };
            btnCancel = new Button
            {
                Text = "Отмена",
                Location = new Point(160, y),
                Width = 80,
                DialogResult = DialogResult.Cancel
            };

            btnOk.Click += BtnOk_Click;
            btnCancel.Click += (s, e) => this.Close();

            this.Controls.AddRange(new Control[] {
                lblCurrentHp,
                lblDamage, numDamage,
                lblWarning,
                btnOk, btnCancel
            });

            this.AcceptButton = btnOk;
            this.CancelButton = btnCancel;
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            DamageAmount = (double)numDamage.Value;

            if (DamageAmount <= 0)
            {
                MessageBox.Show("Урон должен быть больше 0", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
