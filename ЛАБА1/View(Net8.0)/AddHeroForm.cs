using Presenter;
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
    public partial class AddHeroForm : Form
    {
        public string HeroName { get; private set; }
        public int HeroSpeciesId { get; private set; }
        public string HeroGenre { get; private set; }
        public int HeroStrange { get; private set; }
        public string HeroDamageType { get; private set; }
        public double HeroHp { get; private set; }

        private AddHeroPresenter _presenter;
        private ComboBox cmbSpecies;
        private TextBox txtName, txtGenre, txtDamageType;
        private NumericUpDown numStrange, numHp;
        private Button btnOk, btnCancel;

        public AddHeroForm()
        {
            _presenter = new AddHeroPresenter();
            InitializeForm();
            LoadSpecies();
            InitializeComponent();
        }

        private void InitializeForm()
        {
            this.Text = "Добавление нового героя";
            this.Size = new Size(400, 350);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;
            this.MaximizeBox = false;

            int y = 10;
            int labelWidth = 120;
            int controlWidth = 200;

            // Имя героя
            var lblName = new Label { Text = "Имя героя:", Location = new Point(10, y), Width = labelWidth };
            txtName = new TextBox { Location = new Point(140, y - 3), Width = controlWidth };
            y += 30;

            // Раса
            var lblSpecies = new Label { Text = "Раса:", Location = new Point(10, y), Width = labelWidth };
            cmbSpecies = new ComboBox
            {
                Location = new Point(140, y - 3),
                Width = controlWidth,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            y += 30;

            // Гендер
            var lblGenre = new Label { Text = "Гендер:", Location = new Point(10, y), Width = labelWidth };
            txtGenre = new TextBox { Location = new Point(140, y - 3), Width = controlWidth };
            y += 30;

            // Сила
            var lblStrange = new Label { Text = "Сила:", Location = new Point(10, y), Width = labelWidth };
            numStrange = new NumericUpDown
            {
                Location = new Point(140, y - 3),
                Width = 100,
                Minimum = 1,
                Maximum = 1000,
                Value = 50
            };
            y += 30;

            // Тип урона
            var lblDamageType = new Label { Text = "Тип урона:", Location = new Point(10, y), Width = labelWidth };
            txtDamageType = new TextBox { Location = new Point(140, y - 3), Width = controlWidth };
            y += 30;

            // HP
            var lblHp = new Label { Text = "HP:", Location = new Point(10, y), Width = labelWidth };
            numHp = new NumericUpDown
            {
                Location = new Point(140, y - 3),
                Width = 100,
                Minimum = 1,
                Maximum = 10000,
                Value = 100,
                DecimalPlaces = 1
            };
            y += 40;

            // Кнопки
            btnOk = new Button
            {
                Text = "Добавить",
                Location = new Point(100, y),
                Width = 80,
                DialogResult = DialogResult.OK
            };
            btnCancel = new Button
            {
                Text = "Отмена",
                Location = new Point(190, y),
                Width = 80,
                DialogResult = DialogResult.Cancel
            };

            btnOk.Click += BtnOk_Click;
            btnCancel.Click += (s, e) => this.Close();

            this.Controls.AddRange(new Control[] {
                lblName, txtName,
                lblSpecies, cmbSpecies,
                lblGenre, txtGenre,
                lblStrange, numStrange,
                lblDamageType, txtDamageType,
                lblHp, numHp,
                btnOk, btnCancel
            });

            this.AcceptButton = btnOk;
            this.CancelButton = btnCancel;
        }

        private void LoadSpecies()
        {
            var species = _presenter.GetAvailableSpecies();
            cmbSpecies.DisplayMember = "Name";
            cmbSpecies.ValueMember = "Id";
            cmbSpecies.DataSource = species;

            if (species.Count > 0)
                cmbSpecies.SelectedIndex = 0;
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Введите имя героя", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtName.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtGenre.Text))
            {
                MessageBox.Show("Введите гендер", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtGenre.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtDamageType.Text))
            {
                MessageBox.Show("Введите тип урона", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtDamageType.Focus();
                return;
            }

            if (cmbSpecies.SelectedItem == null)
            {
                MessageBox.Show("Выберите расу", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            HeroName = txtName.Text.Trim();
            HeroSpeciesId = (int)cmbSpecies.SelectedValue;
            HeroGenre = txtGenre.Text.Trim();
            HeroStrange = (int)numStrange.Value;
            HeroDamageType = txtDamageType.Text.Trim();
            HeroHp = (double)numHp.Value;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
