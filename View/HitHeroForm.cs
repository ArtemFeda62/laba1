using System;
using System.Drawing;
using System.Reflection.Emit;
using System.Windows.Forms;
using Shared.Interfaces;

namespace View
{
    public partial class HitHeroForm : Form, IHitHeroView
    {
        public event EventHandler<double> ApplyDamage;
        public event EventHandler Cancel;

        private Label lblInfo;
        private NumericUpDown numDamage;
        private Button btnOk;
        private Button btnCancel;

        public double DamageAmount => (double)numDamage.Value;

        public HitHeroForm(string heroName, double currentHp)
        {
            InitializeComponent();
            SetHeroInfo(heroName, currentHp);
        }

        private void InitializeComponent()
        {
            this.Text = "Нанести урон герою";
            this.Size = new Size(300, 180);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            lblInfo = new Label
            {
                Location = new Point(10, 20),
                Width = 280,
                Height = 40,
                TextAlign = ContentAlignment.MiddleCenter
            };

            var lblDamage = new Label { Text = "Урон:", Location = new Point(10, 70), Width = 80 };
            numDamage = new NumericUpDown
            {
                Location = new Point(90, 67),
                Width = 150,
                Minimum = 0,
                Maximum = 10000,
                Value = 10,
                DecimalPlaces = 1
            };

            btnOk = new Button { Text = "OK", Location = new Point(80, 110), Width = 80, DialogResult = DialogResult.OK };
            btnCancel = new Button { Text = "Отмена", Location = new Point(170, 110), Width = 80, DialogResult = DialogResult.Cancel };

            btnOk.Click += (s, e) =>
            {
                if (numDamage.Value > 0)
                {
                    ApplyDamage?.Invoke(this, DamageAmount);
                }
                else
                {
                    MessageBox.Show("Урон должен быть больше 0", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            btnCancel.Click += (s, e) =>
            {
                Cancel?.Invoke(this, EventArgs.Empty);
                this.Close();
            };

            this.Controls.AddRange(new Control[] { lblInfo, lblDamage, numDamage, btnOk, btnCancel });
            this.AcceptButton = btnOk;
            this.CancelButton = btnCancel;
        }

        public void SetHeroInfo(string heroName, double currentHp)
        {
            lblInfo.Text = $"Герой: {heroName}\nТекущее HP: {currentHp:F1}";
            numDamage.Maximum = (decimal)currentHp;
        }

        public new void ShowDialog()
        {
            base.ShowDialog();
        }

        public void CloseDialog()
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}