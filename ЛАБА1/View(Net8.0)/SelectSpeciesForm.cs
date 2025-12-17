// View/SelectSpeciesForm.cs
using Shared.Domain;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace View
{
    public partial class SelectSpeciesForm : Form
    {
        public Species SelectedSpecies { get; private set; }
        public event Action<Species> SpeciesSelected;

        private List<Species> _speciesList;
        private ListBox listBoxSpecies;
        private TextBox txtFilter;
        private Button btnSelect, btnCancel;
        private Label lblCount;

        public SelectSpeciesForm(List<Species> speciesList, string title = "Выбор расы")
        {
            _speciesList = speciesList ?? new List<Species>();
            InitializeForm(title);
        }

        private void InitializeForm(string title)
        {
            this.Text = title;
            this.Size = new Size(500, 400);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;
            this.MaximizeBox = false;

            int y = 10;

            // Заголовок
            var lblTitle = new Label
            {
                Text = title,
                Location = new Point(10, y),
                Width = 400,
                Font = new Font(Font, FontStyle.Bold),
                ForeColor = Color.DarkBlue
            };
            y += 25;

            // Фильтр
            var lblFilter = new Label
            {
                Text = "Фильтр:",
                Location = new Point(10, y),
                Width = 50
            };
            txtFilter = new TextBox
            {
                Location = new Point(60, y - 3),
                Width = 300
            };
            txtFilter.TextChanged += TxtFilter_TextChanged;
            y += 30;

            // Счетчик
            lblCount = new Label
            {
                Text = $"Найдено рас: {_speciesList.Count}",
                Location = new Point(10, y),
                Width = 200,
                ForeColor = Color.DarkGreen
            };
            y += 25;

            // Список рас
            listBoxSpecies = new ListBox
            {
                Location = new Point(10, y),
                Size = new Size(460, 220),
                DisplayMember = "DisplayText",
                ValueMember = "Id",
                Font = new Font("Microsoft Sans Serif", 10)
            };
            listBoxSpecies.DoubleClick += ListBoxSpecies_DoubleClick;
            y += 230;

            // Панель информации
            var infoPanel = new Panel
            {
                Location = new Point(10, y),
                Size = new Size(460, 30),
                BackColor = Color.LightGray,
                BorderStyle = BorderStyle.FixedSingle
            };

            var lblInfo = new Label
            {
                Text = "Двойной клик по элементу для быстрого выбора",
                Location = new Point(5, 5),
                AutoSize = true,
                Font = new Font(Font, FontStyle.Italic)
            };
            infoPanel.Controls.Add(lblInfo);
            y += 40;

            // Кнопки
            btnSelect = new Button
            {
                Text = "✅ Выбрать",
                Location = new Point(150, y),
                Width = 100,
                Enabled = false
            };
            btnCancel = new Button
            {
                Text = "❌ Отмена",
                Location = new Point(260, y),
                Width = 100
            };

            btnSelect.Click += BtnSelect_Click;
            btnCancel.Click += (s, e) =>
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            };

            listBoxSpecies.SelectedIndexChanged += (s, e) =>
            {
                btnSelect.Enabled = listBoxSpecies.SelectedItem != null;
            };

            this.Controls.AddRange(new Control[] {
                lblTitle,
                lblFilter, txtFilter,
                lblCount,
                listBoxSpecies,
                infoPanel,
                btnSelect, btnCancel
            });

            this.AcceptButton = btnSelect;
            this.CancelButton = btnCancel;

            // Загружаем данные
            LoadSpeciesList();
        }

        private void LoadSpeciesList()
        {
            listBoxSpecies.Items.Clear();

            foreach (var species in _speciesList)
            {
                listBoxSpecies.Items.Add(new SpeciesDisplayItem(species));
            }

            UpdateCount();
        }

        private void TxtFilter_TextChanged(object sender, EventArgs e)
        {
            string filter = txtFilter.Text.Trim().ToLower();

            if (string.IsNullOrEmpty(filter))
            {
                LoadSpeciesList();
                return;
            }

            listBoxSpecies.Items.Clear();

            var filtered = _speciesList
                .Where(s => s.Name.ToLower().Contains(filter) ||
                           (s.Description != null && s.Description.ToLower().Contains(filter)))
                .ToList();

            foreach (var species in filtered)
            {
                listBoxSpecies.Items.Add(new SpeciesDisplayItem(species));
            }

            UpdateCount();
        }

        private void UpdateCount()
        {
            lblCount.Text = $"Найдено рас: {listBoxSpecies.Items.Count} из {_speciesList.Count}";
            lblCount.ForeColor = listBoxSpecies.Items.Count == 0 ? Color.Red : Color.DarkGreen;
        }

        private void BtnSelect_Click(object sender, EventArgs e)
        {
            if (listBoxSpecies.SelectedItem != null)
            {
                var displayItem = (SpeciesDisplayItem)listBoxSpecies.SelectedItem;
                SelectedSpecies = displayItem.Species;

                SpeciesSelected?.Invoke(SelectedSpecies);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void ListBoxSpecies_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxSpecies.SelectedItem != null)
            {
                BtnSelect_Click(sender, e);
            }
        }

        // Вспомогательный класс для отображения
        private class SpeciesDisplayItem
        {
            public Species Species { get; }
            public int Id => Species.Id;
            public string DisplayText => $"{Species.Name} (ID: {Species.Id}) - {GetShortDescription()}";

            public SpeciesDisplayItem(Species species)
            {
                Species = species;
            }

            private string GetShortDescription()
            {
                if (string.IsNullOrEmpty(Species.Description))
                    return "Без описания";

                return Species.Description.Length > 50
                    ? Species.Description.Substring(0, 47) + "..."
                    : Species.Description;
            }

            public override string ToString() => DisplayText;
        }
    }
}