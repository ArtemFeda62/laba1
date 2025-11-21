using System;
using System.Drawing;
using System.Windows.Forms;

namespace ЛАБА1
{
    public partial class AddSpeciesForm : Form
    {
        public string SpeciesName { get; private set; }
        public string SpeciesDescription { get; private set; }

        public AddSpeciesForm()
        {
            InitializeForm();
        }

        private void InitializeForm()
        {
            this.Text = "Добавление новой расы";
            this.Size = new System.Drawing.Size(400, 200);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;
            this.MaximizeBox = false;

            var lblName = new Label { Text = "Название расы:", Location = new Point(10, 20), Width = 100 };
            var txtName = new TextBox { Location = new Point(120, 17), Width = 250 };

            var lblDescription = new Label { Text = "Описание:", Location = new Point(10, 50), Width = 100 };
            var txtDescription = new TextBox { Location = new Point(120, 47), Width = 250, Height = 60 };
            txtDescription.Multiline = true;

            var btnOK = new Button { Text = "Добавить", Location = new Point(120, 120), Width = 80 };
            var btnCancel = new Button { Text = "Отмена", Location = new Point(210, 120), Width = 80 };

            btnOK.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtName.Text))
                {
                    MessageBox.Show("Введите название расы", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                SpeciesName = txtName.Text.Trim();
                SpeciesDescription = txtDescription.Text.Trim();
                this.DialogResult = DialogResult.OK;
                this.Close();
            };

            btnCancel.Click += (s, e) =>
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            };

            this.Controls.AddRange(new Control[] { lblName, txtName, lblDescription, txtDescription, btnOK, btnCancel });
        }
    }
}