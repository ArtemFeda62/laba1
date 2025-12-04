using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace View.Forms
{
    public class SpeciesListForm : Form
    {
        public SpeciesListForm(List<Species> species)
        {
            InitializeComponent(species);
        }

        private void InitializeComponent(List<Species> species)
        {
            this.Text = "Список рас";
            this.Size = new Size(500, 400);
            this.StartPosition = FormStartPosition.CenterParent;

            var dataGridView = new DataGridView
            {
                Dock = DockStyle.Fill,
                DataSource = species,
                ReadOnly = true,
                AutoGenerateColumns = false
            };

            dataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Id",
                HeaderText = "ID",
                Width = 50
            });

            dataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Name",
                HeaderText = "Название",
                Width = 150
            });

            dataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Description",
                HeaderText = "Описание",
                Width = 250
            });

            var btnClose = new Button
            {
                Text = "Закрыть",
                Dock = DockStyle.Bottom,
                Height = 40
            };

            btnClose.Click += (s, e) => this.Close();

            var panel = new Panel { Dock = DockStyle.Fill };
            panel.Controls.Add(dataGridView);

            this.Controls.Add(panel);
            this.Controls.Add(btnClose);
        }
    }
}