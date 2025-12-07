using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace View
{
    public partial class SelectSpeciesForm : Form
    {
        public Species SelectedSpecies { get; private set; }

        private List<Species> _speciesList;
        private ListBox listBoxSpecies;
        private Button btnSelect, btnCancel;

        public SelectSpeciesForm(List<Species> speciesList, string title)
        {
            _speciesList = speciesList;
            InitializeComponent(title);
        }

        private void InitializeComponent(string title)
        {
            this.Text = title;
            this.Size = new Size(400, 300);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;
            this.MaximizeBox = false;

            int y = 10;

            // Заголовок
            var lblInfo = new Label
            {
                Text = "Выберите расу:",
                Location = new Point(10, y),
                Width = 200,
                Font = new Font(Font, FontStyle.Bold)
            };
            y += 25;

            // Список рас
            listBoxSpecies = new ListBox
            {
                Location = new Point(10, y),
                Size = new Size(360, 180),
                DisplayMember = "Name",
                ValueMember = "Id"
            };
            listBoxSpecies.DataSource = _speciesList;
            y += 190;

            // Кнопки
            btnSelect = new Button
            {
                Text = "Выбрать",
                Location = new Point(120, y),
                Width = 80,
                DialogResult = DialogResult.OK
            };
            btnCancel = new Button
            {
                Text = "Отмена",
                Location = new Point(210, y),
                Width = 80,
                DialogResult = DialogResult.Cancel
            };

            btnSelect.Click += BtnSelect_Click;
            btnCancel.Click += (s, e) => this.Close();

            this.Controls.AddRange(new Control[] {
                lblInfo,
                listBoxSpecies,
                btnSelect,
                btnCancel
            });

            this.AcceptButton = btnSelect;
            this.CancelButton = btnCancel;
        }

        private void BtnSelect_Click(object sender, EventArgs e)
        {
            if (listBoxSpecies.SelectedItem != null)
            {
                SelectedSpecies = (Species)listBoxSpecies.SelectedItem;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Выберите расу", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
