// View/AddSpeciesForm.cs
using System;
using System.Drawing;
using System.Windows.Forms;

namespace View
{
    public partial class AddSpeciesForm : Form
    {
        public string SpeciesName { get; private set; }
        public string SpeciesDescription { get; private set; }

        private TextBox txtName, txtDescription;
        private Button btnOk, btnCancel;

        public AddSpeciesForm()
        {
            InitializeForm();
        }

        private void InitializeForm()
        {
            this.Text = "Добавление новой расы";
            this.Size = new Size(400, 250);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;
            this.MaximizeBox = false;

            int y = 10;
            int labelWidth = 120;
            int controlWidth = 230;

            // Название расы
            var lblName = new Label
            {
                Text = "Название расы:",
                Location = new Point(10, y),
                Width = labelWidth
            };
            txtName = new TextBox
            {
                Location = new Point(140, y - 3),
                Width = controlWidth
            };
            y += 35;

            // Описание
            var lblDescription = new Label
            {
                Text = "Описание:",
                Location = new Point(10, y),
                Width = labelWidth
            };
            txtDescription = new TextBox
            {
                Location = new Point(140, y - 3),
                Width = controlWidth,
                Height = 80,
                Multiline = true,
                ScrollBars = ScrollBars.Vertical
            };
            y += 100;

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
                lblDescription, txtDescription,
                btnOk, btnCancel
            });

            this.AcceptButton = btnOk;
            this.CancelButton = btnCancel;
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Введите название расы", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtName.Focus();
                return;
            }

            SpeciesName = txtName.Text.Trim();
            SpeciesDescription = txtDescription.Text.Trim();

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}