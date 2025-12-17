// View/AddHeroForm.cs
using System;
using System.Drawing;
using System.Linq;
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

        private ComboBox cmbSpecies;
        private TextBox txtName, txtGenre, txtDamageType;
        private NumericUpDown numStrange, numHp;
        private Button btnOk, btnCancel;

        // Событие для передачи данных обратно в MainForm
        public event Action<AddHeroForm> HeroAdded;

        public AddHeroForm()
        {
            InitializeForm();
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
            var lblName = new Label
            {
                Text = "Имя героя:",
                Location = new Point(10, y),
                Width = labelWidth
            };
            txtName = new TextBox
            {
                Location = new Point(140, y - 3),
                Width = controlWidth
            };
            y += 30;

            // Раса
            var lblSpecies = new Label
            {
                Text = "Раса:",
                Location = new Point(10, y),
                Width = labelWidth
            };
            cmbSpecies = new ComboBox
            {
                Location = new Point(140, y - 3),
                Width = controlWidth,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            // В реальном приложении список рас загружается через Presenter
            // Здесь просто тестовые данные
            cmbSpecies.Items.AddRange(new object[]
            {
                new { Id = 1, Name = "Человек" },
                new { Id = 2, Name = "Эльф" },
                new { Id = 3, Name = "Гном" },
                new { Id = 4, Name = "Орк" },
                new { Id = 5, Name = "Драконорожденный" }
            });
            cmbSpecies.DisplayMember = "Name";
            cmbSpecies.ValueMember = "Id";
            if (cmbSpecies.Items.Count > 0)
                cmbSpecies.SelectedIndex = 0;
            y += 30;

            // Гендер
            var lblGenre = new Label
            {
                Text = "Гендер:",
                Location = new Point(10, y),
                Width = labelWidth
            };
            txtGenre = new TextBox
            {
                Location = new Point(140, y - 3),
                Width = controlWidth
            };
            y += 30;

            // Сила
            var lblStrange = new Label
            {
                Text = "Сила:",
                Location = new Point(10, y),
                Width = labelWidth
            };
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
            var lblDamageType = new Label
            {
                Text = "Тип урона:",
                Location = new Point(10, y),
                Width = labelWidth
            };
            txtDamageType = new TextBox
            {
                Location = new Point(140, y - 3),
                Width = controlWidth
            };
            y += 30;

            // HP
            var lblHp = new Label
            {
                Text = "HP:",
                Location = new Point(10, y),
                Width = labelWidth
            };
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
                Width = 80
            };
            btnCancel = new Button
            {
                Text = "Отмена",
                Location = new Point(190, y),
                Width = 80
            };

            btnOk.Click += BtnOk_Click;
            btnCancel.Click += (s, e) =>
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            };

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

        private void BtnOk_Click(object sender, EventArgs e)
        {
            if (!ValidateInput())
                return;

            // Сохраняем данные
            HeroName = txtName.Text.Trim();

            if (cmbSpecies.SelectedItem != null)
            {
                dynamic selectedItem = cmbSpecies.SelectedItem;
                HeroSpeciesId = selectedItem.Id;
            }

            HeroGenre = txtGenre.Text.Trim();
            HeroStrange = (int)numStrange.Value;
            HeroDamageType = txtDamageType.Text.Trim();
            HeroHp = (double)numHp.Value;

            // Вызываем событие
            HeroAdded?.Invoke(this);

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Введите имя героя", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtName.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtGenre.Text))
            {
                MessageBox.Show("Введите гендер", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtGenre.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtDamageType.Text))
            {
                MessageBox.Show("Введите тип урона", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtDamageType.Focus();
                return false;
            }

            if (cmbSpecies.SelectedItem == null)
            {
                MessageBox.Show("Выберите расу", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        // Метод для установки списка рас (вызывается из MainForm через Presenter)
        public void SetSpeciesList(object[] speciesList)
        {
            if (cmbSpecies.InvokeRequired)
            {
                cmbSpecies.Invoke(new Action<object[]>(SetSpeciesList), speciesList);
                return;
            }

            cmbSpecies.Items.Clear();
            cmbSpecies.Items.AddRange(speciesList);

            if (cmbSpecies.Items.Count > 0)
                cmbSpecies.SelectedIndex = 0;
        }
    }
}