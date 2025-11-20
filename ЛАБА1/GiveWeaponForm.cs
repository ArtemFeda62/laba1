using System;
using System.Drawing;
using System.Windows.Forms;

namespace ЛАБА1
{
    public partial class GiveWeaponForm : Form
    {
        public string WeaponName { get; private set; }
        public int DamageBonus { get; private set; }

        private TextBox txtWeaponName;
        private TextBox txtDamageBonus;
        private Button btnOk;
        private Button btnCancel;

        public GiveWeaponForm()
        {
            BuildForm();
        }

        private void BuildForm()
        {
            this.Text = "Выдать оружие герою";
            this.Size = new Size(300, 200);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            var label1 = new Label
            {
                Text = "Название оружия:",
                Location = new Point(20, 20),
                Size = new Size(100, 20)
            };

            txtWeaponName = new TextBox
            {
                Location = new Point(120, 20),
                Size = new Size(150, 20),
                Text = "Меч"
            };

            var label2 = new Label
            {
                Text = "Бонус к силе:",
                Location = new Point(20, 60),
                Size = new Size(100, 20)
            };

            txtDamageBonus = new TextBox
            {
                Location = new Point(120, 60),
                Size = new Size(150, 20),
                Text = "10"
            };

            btnOk = new Button
            {
                Text = "Выдать",
                Location = new Point(60, 100),
                Size = new Size(80, 30),
                DialogResult = DialogResult.OK
            };

            btnCancel = new Button
            {
                Text = "Отмена",
                Location = new Point(160, 100),
                Size = new Size(80, 30),
                DialogResult = DialogResult.Cancel
            };

            btnOk.Click += BtnOk_Click;
            btnCancel.Click += BtnCancel_Click;

            this.Controls.AddRange(new Control[] {
                label1, txtWeaponName,
                label2, txtDamageBonus,
                btnOk, btnCancel
            });
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtWeaponName.Text))
            {
                MessageBox.Show("Введите название оружия", "Ошибка",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtDamageBonus.Text, out int bonus) || bonus <= 0)
            {
                MessageBox.Show("Введите корректный бонус к силе", "Ошибка",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            WeaponName = txtWeaponName.Text;
            DamageBonus = bonus;
            DialogResult = DialogResult.OK;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}