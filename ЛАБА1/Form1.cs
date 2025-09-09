using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ЛАБА1
{
    public partial class Form1 : Form
    {
        private Logic logic;
        private BindingSource heroesBindingSource;

        public Form1()
        {
            InitializeComponent();
            logic = new Logic();
            heroesBindingSource = new BindingSource();
            InitializeDataGridView();
        }

        private void InitializeDataGridView()
        {
            // Настройка DataGridView
            dataGridViewHeroes.AutoGenerateColumns = false;
            dataGridViewHeroes.DataSource = heroesBindingSource;

            // Добавление колонок
            dataGridViewHeroes.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Id",
                HeaderText = "ID",
                Width = 50
            });

            dataGridViewHeroes.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Name",
                HeaderText = "Имя",
                Width = 150
            });

            dataGridViewHeroes.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Species",
                HeaderText = "Раса",
                Width = 100
            });

            dataGridViewHeroes.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Genre",
                HeaderText = "Гендер",
                Width = 80
            });

            dataGridViewHeroes.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Strange",
                HeaderText = "Сила",
                Width = 60
            });

            dataGridViewHeroes.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Hp",
                HeaderText = "HP",
                Width = 70
            });

            dataGridViewHeroes.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "TypeOfDamage",
                HeaderText = "Тип урона",
                Width = 120
            });
        }

        private void LoadHeroes()
        {
            var heroes = logic.GetListHeros();
            heroesBindingSource.DataSource = heroes;
            lblTotalCount.Text = $"Всего героев: {heroes.Count}";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadHeroes();
        }

        private void btnAddHero_Click(object sender, EventArgs e)
        {
            using (var addForm = new AddEditHeroForm())
            {
                if (addForm.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        logic.CreateHero(
                            addForm.HeroName,
                            addForm.Genre,
                            addForm.Species,
                            addForm.Hp,
                            addForm.DamageType,
                            addForm.Strange
                        );
                        LoadHeroes();
                        MessageBox.Show("Герой успешно добавлен!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при добавлении героя: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnEditHero_Click(object sender, EventArgs e)
        {
            if (dataGridViewHeroes.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите героя для редактирования", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selectedHero = (Hero)dataGridViewHeroes.SelectedRows[0].DataBoundItem;

            using (var editForm = new AddEditHeroForm(selectedHero))
            {
                if (editForm.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        var updatedHero = new Hero(
                            editForm.HeroName,
                            editForm.Species,
                            editForm.Genre,
                            editForm.Strange,
                            editForm.DamageType
                        )
                        {
                            Id = selectedHero.Id,
                            Hp = editForm.Hp
                        };

                        logic.UpdateHero(updatedHero);
                        LoadHeroes();
                        MessageBox.Show("Герой успешно обновлен!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при обновлении героя: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnDeleteHero_Click(object sender, EventArgs e)
        {
            if (dataGridViewHeroes.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите героя для удаления", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selectedHero = (Hero)dataGridViewHeroes.SelectedRows[0].DataBoundItem;

            var result = MessageBox.Show(
                $"Вы уверены, что хотите удалить героя '{selectedHero.Name}'?",
                "Подтверждение удаления",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                try
                {
                    logic.KillHero(selectedHero.Id);
                    LoadHeroes();
                    MessageBox.Show("Герой успешно удален!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении героя: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnHitHero_Click(object sender, EventArgs e)
        {
            if (dataGridViewHeroes.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите героя для нанесения урона", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selectedHero = (Hero)dataGridViewHeroes.SelectedRows[0].DataBoundItem;

            using (var hitForm = new HitHeroForm(selectedHero))
            {
                if (hitForm.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        logic.HitHero(selectedHero.Id, hitForm.Damage);
                        LoadHeroes();

                        var updatedHero = logic.GetHero(selectedHero.Id);
                        if (updatedHero.Hp <= 0)
                        {
                            MessageBox.Show("Герой получил смертельный урон и был удален!", "Урон", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            MessageBox.Show($"Урон нанесен! Новое HP: {updatedHero.Hp}", "Урон", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при нанесении урона: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            var searchText = txtSearch.Text.Trim();
            if (string.IsNullOrEmpty(searchText))
            {
                LoadHeroes();
                return;
            }

            var foundHeroes = logic.FindHeroesByName(searchText);
            heroesBindingSource.DataSource = foundHeroes;
            lblTotalCount.Text = $"Найдено героев: {foundHeroes.Count}";
        }

        private void btnGroupBySpecies_Click(object sender, EventArgs e)
        {
            var groupedHeroes = logic.GroupHeroesBySpecies();
            ShowGroupedResults("Группировка по расам", groupedHeroes);
        }

        private void btnGroupByDamage_Click(object sender, EventArgs e)
        {
            var groupedHeroes = logic.GroupHeroesByDamageType();
            ShowGroupedResults("Группировка по типу урона", groupedHeroes);
        }

        private void btnLowHp_Click(object sender, EventArgs e)
        {
            var lowHpHeroes = logic.GetHeroesWithLowHp(50);
            heroesBindingSource.DataSource = lowHpHeroes;
            lblTotalCount.Text = $"Героев с HP ≤ 50: {lowHpHeroes.Count}";
        }

        private void btnStrongest_Click(object sender, EventArgs e)
        {
            var strongestHeroes = logic.GetStrongestHeroes(3);
            heroesBindingSource.DataSource = strongestHeroes;
            lblTotalCount.Text = "Топ-3 самых сильных героя";
        }

        private void ShowGroupedResults(string title, Dictionary<string, List<Hero>> groupedData)
        {
            var sb = new StringBuilder();
            sb.AppendLine(title);
            sb.AppendLine("======================");

            foreach (var group in groupedData)
            {
                sb.AppendLine($"\n--- {group.Key} ({group.Value.Count}) ---");
                foreach (var hero in group.Value)
                {
                    sb.AppendLine($"  {hero.Name} - HP: {hero.Hp}, Сила: {hero.Strange}");
                }
            }

            MessageBox.Show(sb.ToString(), title, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadHeroes();
            txtSearch.Clear();
        }

        private void dataGridViewHeroes_SelectionChanged(object sender, EventArgs e)
        {
            bool hasSelection = dataGridViewHeroes.SelectedRows.Count > 0;
            btnEditHero.Enabled = hasSelection;
            btnDeleteHero.Enabled = hasSelection;
            btnHitHero.Enabled = hasSelection;
        }

        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnSearch.PerformClick();
                e.Handled = true;
            }
        }
    }
}
