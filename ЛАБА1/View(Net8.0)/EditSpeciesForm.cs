// View/EditSpeciesForm.cs
using Shared.Domain;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace View
{
    public partial class EditSpeciesForm : Form
    {
        public string SpeciesName { get; private set; }
        public string SpeciesDescription { get; private set; }
        public bool IsUpdated { get; private set; }

        private Species _species;
        private TextBox txtName, txtDescription;
        private Button btnSave, btnCancel;

        public EditSpeciesForm(Species species)
        {
            _species = species;
            IsUpdated = false;
            InitializeForm();
            LoadSpeciesData();
        }

        private void InitializeForm()
        {
            this.Text = "Редактирование расы";
            this.Size = new Size(450, 300);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;
            this.MaximizeBox = false;

            int y = 10;

            // Заголовок
            var lblTitle = new Label
            {
                Text = $"Редактирование расы (ID: {_species.Id})",
                Location = new Point(10, y),
                Width = 400,
                Font = new Font(Font, FontStyle.Bold),
                ForeColor = Color.DarkBlue
            };
            y += 30;

            // Название расы
            var lblName = new Label
            {
                Text = "Название расы:",
                Location = new Point(10, y),
                Width = 120
            };
            txtName = new TextBox
            {
                Location = new Point(140, y - 3),
                Width = 280
            };
            y += 35;

            // Описание
            var lblDescription = new Label
            {
                Text = "Описание:",
                Location = new Point(10, y),
                Width = 120
            };
            txtDescription = new TextBox
            {
                Location = new Point(140, y - 3),
                Width = 280,
                Height = 120,
                Multiline = true,
                ScrollBars = ScrollBars.Vertical
            };
            y += 130;

            // Информационная панель
            var infoPanel = new Panel
            {
                Location = new Point(10, y),
                Size = new Size(410, 30),
                BackColor = Color.LightYellow,
                BorderStyle = BorderStyle.FixedSingle
            };

            var lblInfo = new Label
            {
                Text = "⚠️ Изменения вступят в силу после сохранения",
                Location = new Point(5, 5),
                AutoSize = true,
                ForeColor = Color.DarkOrange
            };
            infoPanel.Controls.Add(lblInfo);
            y += 40;

            // Кнопки
            btnSave = new Button
            {
                Text = "💾 Сохранить",
                Location = new Point(120, y),
                Width = 100,
                BackColor = Color.LightGreen
            };
            btnCancel = new Button
            {
                Text = "❌ Отмена",
                Location = new Point(230, y),
                Width = 100
            };

            btnSave.Click += BtnSave_Click;
            btnCancel.Click += (s, e) =>
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            };

            this.Controls.AddRange(new Control[] {
                lblTitle,
                lblName, txtName,
                lblDescription, txtDescription,
                infoPanel,
                btnSave, btnCancel
            });

            this.AcceptButton = btnSave;
            this.CancelButton = btnCancel;
        }

        private void LoadSpeciesData()
        {
            txtName.Text = _species.Name;
            txtDescription.Text = _species.Description;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Введите название расы", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtName.Focus();
                return;
            }

            // Проверяем, были ли изменения
            if (txtName.Text.Trim() == _species.Name &&
                txtDescription.Text.Trim() == _species.Description)
            {
                MessageBox.Show("Нет изменений для сохранения", "Информация",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            SpeciesName = txtName.Text.Trim();
            SpeciesDescription = txtDescription.Text.Trim();
            IsUpdated = true;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}