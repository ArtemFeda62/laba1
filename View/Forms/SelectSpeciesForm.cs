using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace View.Forms
{
    public partial class SelectSpeciesForm : Form
    {
        public Species SelectedSpecies { get; private set; }

        public SelectSpeciesForm(List<Species> speciesList, string title)
        {
            InitializeComponent(speciesList, title);
        }

        private void InitializeComponent(List<Species> speciesList, string title)
        {
            this.Text = title;
            this.Size = new Size(400, 300);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;
            this.MaximizeBox = false;

            var lblInfo = new Label
            {
                Text = "Выберите расу:",
                Location = new Point(10, 10),
                Width = 200
            };

            var listBoxSpecies = new ListBox
            {
                Location = new Point(10, 30),
                Size = new Size(360, 180),
                DisplayMember = "Name"
            };
            listBoxSpecies.DataSource = speciesList;

            var btnSelect = new Button
            {
                Text = "Выбрать",
                Location = new Point(120, 220),
                Width = 80,
                DialogResult = DialogResult.OK
            };

            var btnCancel = new Button
            {
                Text = "Отмена",
                Location = new Point(210, 220),
                Width = 80,
                DialogResult = DialogResult.Cancel
            };

            btnSelect.Click += (s, e) =>
            {
                if (listBoxSpecies.SelectedItem != null)
                {
                    SelectedSpecies = (Species)listBoxSpecies.SelectedItem;
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Выберите расу", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            btnCancel.Click += (s, e) =>
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            };

            this.Controls.AddRange(new Control[] {
                lblInfo, listBoxSpecies, btnSelect, btnCancel
            });

            this.AcceptButton = btnSelect;
            this.CancelButton = btnCancel;
        }
    }
}