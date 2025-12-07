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
    public partial class MainForm : Form, IView
    {
        private MainPresenter _presenter;
        private DataGridView dataGridView1;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel statusLabel;
        private Panel paginationPanel;
        private Label lblPageInfo;
        private ComboBox cmbPageSize;
        private Button btnFirst, btnPrev, btnNext, btnLast;

        public MainForm()
        {
            InitializeComponent();
            _presenter = new MainPresenter(this);
            InitializeControls();
            _presenter.Initialize();
        }

        private void InitializeComponent()
        {
            this.Text = "Управление героями";
            this.Size = new Size(900, 600);
            this.StartPosition = FormStartPosition.CenterScreen;

            dataGridView1 = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false
            };

            statusStrip1 = new StatusStrip();
            statusLabel = new ToolStripStatusLabel();
            statusStrip1.Items.Add(statusLabel);

            this.Controls.Add(dataGridView1);
            this.Controls.Add(statusStrip1);
        }

        private void InitializeControls()
        {
            // Меню
            var menuStrip = new MenuStrip();

            var fileMenu = new ToolStripMenuItem("Файл");
            var exitItem = new ToolStripMenuItem("Выход");
            exitItem.Click += (s, e) => Application.Exit();
            fileMenu.DropDownItems.Add(exitItem);

            var heroesMenu = new ToolStripMenuItem("Герои");
            var addHeroItem = new ToolStripMenuItem("Добавить героя");
            var deleteHeroItem = new ToolStripMenuItem("Удалить героя");
            var hitHeroItem = new ToolStripMenuItem("Нанести урон");
            var refreshItem = new ToolStripMenuItem("Обновить список");

            addHeroItem.Click += BtnAddHero_Click;
            deleteHeroItem.Click += BtnDeleteHero_Click;
            hitHeroItem.Click += BtnHitHero_Click;
            refreshItem.Click += BtnRefresh_Click;

            heroesMenu.DropDownItems.Add(addHeroItem);
            heroesMenu.DropDownItems.Add(deleteHeroItem);
            heroesMenu.DropDownItems.Add(hitHeroItem);
            heroesMenu.DropDownItems.Add(new ToolStripSeparator());
            heroesMenu.DropDownItems.Add(refreshItem);

            var speciesMenu = new ToolStripMenuItem("Расы");
            var addSpeciesItem = new ToolStripMenuItem("Добавить расу");
            var showSpeciesItem = new ToolStripMenuItem("Показать расы");
            var editSpeciesItem = new ToolStripMenuItem("Редактировать расу");
            var deleteSpeciesItem = new ToolStripMenuItem("Удалить расу");

            addSpeciesItem.Click += BtnAddSpecies_Click;
            showSpeciesItem.Click += BtnShowSpecies_Click;
            editSpeciesItem.Click += BtnEditSpecies_Click;
            deleteSpeciesItem.Click += BtnDeleteSpecies_Click;

            speciesMenu.DropDownItems.Add(addSpeciesItem);
            speciesMenu.DropDownItems.Add(showSpeciesItem);
            speciesMenu.DropDownItems.Add(new ToolStripSeparator());
            speciesMenu.DropDownItems.Add(editSpeciesItem);
            speciesMenu.DropDownItems.Add(deleteSpeciesItem);

            var viewMenu = new ToolStripMenuItem("Вид");
            var statisticsItem = new ToolStripMenuItem("Статистика");
            var groupBySpeciesItem = new ToolStripMenuItem("Группировка по расам");
            var groupByDamageItem = new ToolStripMenuItem("Группировка по типу урона");
            var showWoundedItem = new ToolStripMenuItem("Раненые герои");
            var showStrongestItem = new ToolStripMenuItem("Топ-3 сильнейших");

            statisticsItem.Click += BtnStatistics_Click;
            groupBySpeciesItem.Click += BtnGroupBySpecies_Click;
            groupByDamageItem.Click += BtnGroupByDamage_Click;
            showWoundedItem.Click += BtnShowWounded_Click;
            showStrongestItem.Click += BtnShowStrongest_Click;

            viewMenu.DropDownItems.Add(statisticsItem);
            viewMenu.DropDownItems.Add(new ToolStripSeparator());
            viewMenu.DropDownItems.Add(groupBySpeciesItem);
            viewMenu.DropDownItems.Add(groupByDamageItem);
            viewMenu.DropDownItems.Add(showWoundedItem);
            viewMenu.DropDownItems.Add(showStrongestItem);

            menuStrip.Items.Add(fileMenu);
            menuStrip.Items.Add(heroesMenu);
            menuStrip.Items.Add(speciesMenu);
            menuStrip.Items.Add(viewMenu);

            this.MainMenuStrip = menuStrip;
            this.Controls.Add(menuStrip);

            // Панель инструментов
            var toolStrip = new ToolStrip();
            var btnAdd = new ToolStripButton("Добавить героя");
            var btnDelete = new ToolStripButton("Удалить");
            var btnRefresh = new ToolStripButton("Обновить");
            var btnStats = new ToolStripButton("Статистика");

            btnAdd.Click += BtnAddHero_Click;
            btnDelete.Click += BtnDeleteHero_Click;
            btnRefresh.Click += BtnRefresh_Click;
            btnStats.Click += BtnStatistics_Click;

            toolStrip.Items.Add(btnAdd);
            toolStrip.Items.Add(btnDelete);
            toolStrip.Items.Add(new ToolStripSeparator());
            toolStrip.Items.Add(btnRefresh);
            toolStrip.Items.Add(new ToolStripSeparator());
            toolStrip.Items.Add(btnStats);

            this.Controls.Add(toolStrip);

            // Панель пагинации
            InitializePaginationPanel();

            // Настройка DataGridView
            ConfigureDataGridView();
        }

        private void ConfigureDataGridView()
        {
            dataGridView1.Columns.Clear();
            dataGridView1.AutoGenerateColumns = false;

            // Настройка колонок
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Id",
                DataPropertyName = "Id",
                HeaderText = "ID",
                Width = 50,
                ReadOnly = true
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Name",
                DataPropertyName = "Name",
                HeaderText = "Имя героя",
                Width = 150,
                ReadOnly = true
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "SpeciesName",
                DataPropertyName = "SpeciesName",
                HeaderText = "Раса",
                Width = 100,
                ReadOnly = true
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Hp",
                DataPropertyName = "Hp",
                HeaderText = "HP",
                Width = 80,
                ReadOnly = true,
                DefaultCellStyle = new DataGridViewCellStyle() { Format = "F1" }
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Strange",
                DataPropertyName = "Strange",
                HeaderText = "Сила",
                Width = 60,
                ReadOnly = true
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Genre",
                DataPropertyName = "Genre",
                HeaderText = "Гендер",
                Width = 80,
                ReadOnly = true
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "TypeOfDamage",
                DataPropertyName = "TypeOfDamage",
                HeaderText = "Тип урона",
                Width = 150,
                ReadOnly = true
            });

            // Стиль для строк с низким HP
            dataGridView1.CellFormatting += DataGridView1_CellFormatting;
        }

        private void DataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex >= 0 && dataGridView1.Rows[e.RowIndex].DataBoundItem != null)
            {
                var row = dataGridView1.Rows[e.RowIndex];
                var heroData = row.DataBoundItem as dynamic;

                if (heroData != null && heroData.Hp < 50)
                {
                    row.DefaultCellStyle.BackColor = Color.LightPink;
                    row.DefaultCellStyle.ForeColor = Color.DarkRed;
                }
                else if (heroData != null && heroData.Hp <= 0)
                {
                    row.DefaultCellStyle.BackColor = Color.Gray;
                    row.DefaultCellStyle.ForeColor = Color.White;
                }
            }
        }

        private void InitializePaginationPanel()
        {
            paginationPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 40,
                BackColor = SystemColors.Control
            };

            var lblPageSize = new Label
            {
                Text = "На странице:",
                Location = new Point(10, 10),
                AutoSize = true
            };

            cmbPageSize = new ComboBox
            {
                Location = new Point(90, 7),
                Width = 60,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbPageSize.Items.AddRange(new object[] { 5, 10, 20, 50 });
            cmbPageSize.SelectedItem = 10;
            cmbPageSize.SelectedIndexChanged += (s, e) =>
            {
                if (cmbPageSize.SelectedItem != null)
                    _presenter.SetPageSize((int)cmbPageSize.SelectedItem);
            };

            btnFirst = new Button { Text = "«", Location = new Point(160, 7), Width = 30 };
            btnPrev = new Button { Text = "‹", Location = new Point(195, 7), Width = 30 };
            lblPageInfo = new Label { Text = "1 / 1", Location = new Point(230, 10), AutoSize = true };
            btnNext = new Button { Text = "›", Location = new Point(270, 7), Width = 30 };
            btnLast = new Button { Text = "»", Location = new Point(305, 7), Width = 30 };

            btnFirst.Click += (s, e) => _presenter.GoToFirstPage();
            btnPrev.Click += (s, e) => _presenter.GoToPreviousPage();
            btnNext.Click += (s, e) => _presenter.GoToNextPage();
            btnLast.Click += (s, e) => _presenter.GoToLastPage();

            paginationPanel.Controls.AddRange(new Control[]
            {
            lblPageSize, cmbPageSize,
            btnFirst, btnPrev, lblPageInfo, btnNext, btnLast
            });

            this.Controls.Add(paginationPanel);
        }

        // Реализация интерфейса IView
        public void RefreshHeroesList()
        {
            try
            {
                // Получаем данные из презентера (в реальной реализации)
                // и обновляем DataGridView
                var heroes = _presenter.GetAllHeroes(); // Этот метод нужно добавить в Presenter

                dataGridView1.DataSource = heroes?.Select(h => new
                {
                    h.Id,
                    h.Name,
                    SpeciesName = h.Species?.Name ?? "Неизвестно",
                    h.Hp,
                    h.Strange,
                    h.Genre,
                    h.TypeOfDamage
                }).ToList();

                UpdatePaginationInfo();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обновлении списка: {ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void UpdateStatusBar(string status)
        {
            statusLabel.Text = status;
        }

        private void UpdatePaginationInfo()
        {
            lblPageInfo.Text = $"{_presenter.CurrentPage} / {_presenter.TotalPages}";

            btnFirst.Enabled = _presenter.CanGoToPreviousPage;
            btnPrev.Enabled = _presenter.CanGoToPreviousPage;
            btnNext.Enabled = _presenter.CanGoToNextPage;
            btnLast.Enabled = _presenter.CanGoToNextPage;
        }

        // Обработчики событий
        private void BtnAddHero_Click(object sender, EventArgs e)
        {
            using (var form = new AddHeroForm())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    _presenter.AddHero(form.HeroName, form.HeroSpeciesId, form.HeroGenre,
                                     form.HeroStrange, form.HeroDamageType, form.HeroHp);
                    RefreshHeroesList();
                }
            }
        }

        private void BtnDeleteHero_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null && dataGridView1.CurrentRow.DataBoundItem != null)
            {
                try
                {
                    var heroData = dataGridView1.CurrentRow.DataBoundItem as dynamic;
                    if (heroData != null)
                    {
                        var result = MessageBox.Show($"Удалить героя '{heroData.Name}'?",
                            "Подтверждение удаления",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question);

                        if (result == DialogResult.Yes)
                        {
                            _presenter.DeleteHero(heroData.Id);
                            RefreshHeroesList();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении: {ex.Message}",
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Выберите героя для удаления",
                    "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnHitHero_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null && dataGridView1.CurrentRow.DataBoundItem != null)
            {
                try
                {
                    var heroData = dataGridView1.CurrentRow.DataBoundItem as dynamic;
                    if (heroData != null)
                    {
                        using (var form = new HitHeroForm(heroData))
                        {
                            if (form.ShowDialog() == DialogResult.OK)
                            {
                                _presenter.HitHero(heroData.Id, form.DamageAmount);
                                RefreshHeroesList();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при нанесении урона: {ex.Message}",
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Выберите героя для нанесения урона",
                    "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            RefreshHeroesList();
        }

        private void BtnAddSpecies_Click(object sender, EventArgs e)
        {
            using (var form = new AddSpeciesForm())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    _presenter.AddSpecies(form.SpeciesName, form.SpeciesDescription);
                    MessageBox.Show("Раса успешно добавлена", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void BtnShowSpecies_Click(object sender, EventArgs e)
        {
            var species = _presenter.GetAllSpecies();
            ShowSpeciesList(species);
        }

        private void BtnEditSpecies_Click(object sender, EventArgs e)
        {
            var speciesList = _presenter.GetAllSpecies();
            if (!speciesList.Any())
            {
                MessageBox.Show("Нет доступных рас для редактирования",
                    "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (var form = new SelectSpeciesForm(speciesList, "Редактирование расы"))
            {
                if (form.ShowDialog() == DialogResult.OK && form.SelectedSpecies != null)
                {
                    using (var editForm = new EditSpeciesForm(form.SelectedSpecies))
                    {
                        if (editForm.ShowDialog() == DialogResult.OK)
                        {
                            var updatedSpecies = new Species
                            {
                                Id = form.SelectedSpecies.Id,
                                Name = editForm.SpeciesName,
                                Description = editForm.SpeciesDescription
                            };

                            _presenter.UpdateSpecies(updatedSpecies);
                            MessageBox.Show("Раса успешно обновлена", "Успех",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
        }

        private void BtnDeleteSpecies_Click(object sender, EventArgs e)
        {
            var speciesList = _presenter.GetAllSpecies();
            if (!speciesList.Any())
            {
                MessageBox.Show("Нет доступных рас для удаления",
                    "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (var form = new SelectSpeciesForm(speciesList, "Удаление расы"))
            {
                if (form.ShowDialog() == DialogResult.OK && form.SelectedSpecies != null)
                {
                    var result = MessageBox.Show($"Удалить расу '{form.SelectedSpecies.Name}'?",
                        "Подтверждение удаления",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        try
                        {
                            _presenter.DeleteSpecies(form.SelectedSpecies.Id);
                            MessageBox.Show("Раса успешно удалена", "Успех",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Ошибка при удалении: {ex.Message}",
                                "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        private void BtnStatistics_Click(object sender, EventArgs e)
        {
            var stats = _presenter.GetStatistics();
            ShowStatistics(stats);
        }

        private void BtnGroupBySpecies_Click(object sender, EventArgs e)
        {
            var grouped = _presenter.GroupHeroesBySpecies();
            ShowGroupedHeroes(grouped, "Герои по расам");
        }

        private void BtnGroupByDamage_Click(object sender, EventArgs e)
        {
            var grouped = _presenter.GroupHeroesByDamageType();
            ShowGroupedHeroes(grouped, "Герои по типу урона");
        }

        private void BtnShowWounded_Click(object sender, EventArgs e)
        {
            var wounded = _presenter.GetWoundedHeroes();
            ShowHeroesList(wounded, "Раненые герои (HP < 50)");
        }

        private void BtnShowStrongest_Click(object sender, EventArgs e)
        {
            var strongest = _presenter.GetStrongestHeroes(3);
            ShowHeroesList(strongest, "Топ-3 самых сильных героя");
        }

        // Вспомогательные методы отображения
        private void ShowSpeciesList(List<Species> species)
        {
            if (!species.Any())
            {
                MessageBox.Show("Нет доступных рас",
                    "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var form = new Form
            {
                Text = "Все расы",
                Size = new Size(500, 400),
                StartPosition = FormStartPosition.CenterParent,
                MaximizeBox = false
            };

            var grid = new DataGridView
            {
                Dock = DockStyle.Fill,
                DataSource = species,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };

            var btnClose = new Button
            {
                Text = "Закрыть",
                Dock = DockStyle.Bottom,
                Height = 30
            };
            btnClose.Click += (s, e) => form.Close();

            var panel = new Panel { Dock = DockStyle.Fill };
            panel.Controls.Add(grid);

            form.Controls.Add(panel);
            form.Controls.Add(btnClose);
            form.ShowDialog();
        }

        private void ShowStatistics(HeroStatistics stats)
        {
            var form = new Form
            {
                Text = "Статистика героев",
                Size = new Size(650, 500),
                StartPosition = FormStartPosition.CenterParent,
                MaximizeBox = false,
                FormBorderStyle = FormBorderStyle.FixedDialog
            };

            var textBox = new TextBox
            {
                Multiline = true,
                Dock = DockStyle.Fill,
                ScrollBars = ScrollBars.Vertical,
                Font = new Font("Consolas", 9),
                ReadOnly = true,
                BackColor = Color.White
            };

            textBox.Text = FormatStatistics(stats);
            form.Controls.Add(textBox);
            form.ShowDialog();
        }

        private string FormatStatistics(HeroStatistics stats)
        {
            var sb = new System.Text.StringBuilder();
            sb.AppendLine("═══════════════════════════════════════");
            sb.AppendLine("             СТАТИСТИКА ГЕРОЕВ");
            sb.AppendLine("═══════════════════════════════════════");
            sb.AppendLine();

            sb.AppendLine("ОБЩАЯ СТАТИСТИКА:");
            sb.AppendLine($"Всего героев: {stats.TotalHeroes}");
            sb.AppendLine($"Средняя сила: {stats.AverageStrength:F2}");
            sb.AppendLine($"Среднее HP: {stats.AverageHp:F2}");
            sb.AppendLine($"Максимальная сила: {stats.MaxStrength}");
            sb.AppendLine($"Минимальное HP: {stats.MinHp:F2}");
            sb.AppendLine();

            sb.AppendLine("СТАТИСТИКА ПО РАСАМ:");
            if (stats.SpeciesStats != null && stats.SpeciesStats.Any())
            {
                foreach (var stat in stats.SpeciesStats)
                {
                    sb.AppendLine($"  {stat.Species}:");
                    sb.AppendLine($"    Количество: {stat.Count} героев");
                    sb.AppendLine($"    Средняя сила: {stat.AvgStrength:F1}");
                    sb.AppendLine($"    Среднее HP: {stat.AvgHp:F1}");
                }
            }
            else
            {
                sb.AppendLine("  Нет данных");
            }
            sb.AppendLine();

            sb.AppendLine("СТАТИСТИКА ПО ТИПАМ УРОНА:");
            if (stats.DamageTypeStats != null && stats.DamageTypeStats.Any())
            {
                foreach (var stat in stats.DamageTypeStats)
                {
                    sb.AppendLine($"  {stat.DamageType}:");
                    sb.AppendLine($"    Количество: {stat.Count} героев");
                    sb.AppendLine($"    Общая сила: {stat.TotalStrength}");
                }
            }
            else
            {
                sb.AppendLine("  Нет данных");
            }
            sb.AppendLine();

            sb.AppendLine("СТАТИСТИКА ПО ГЕНДЕРАМ:");
            if (stats.GenderStats != null && stats.GenderStats.Any())
            {
                foreach (var stat in stats.GenderStats)
                {
                    sb.AppendLine($"  {stat.Gender}: {stat.Count} героев ({stat.Percentage:F1}%)");
                }
            }
            else
            {
                sb.AppendLine("  Нет данных");
            }
            sb.AppendLine();

            sb.AppendLine($"ГЕРОИ С НИЗКИМ HP (<50): {stats.LowHpHeroes?.Count ?? 0}");
            if (stats.LowHpHeroes != null && stats.LowHpHeroes.Any())
            {
                foreach (var hero in stats.LowHpHeroes)
                {
                    sb.AppendLine($"  {hero.Name} - {hero.Hp:F1} HP ({hero.Species?.Name})");
                }
            }
            sb.AppendLine();
            sb.AppendLine("═══════════════════════════════════════");

            return sb.ToString();
        }

        private void ShowGroupedHeroes(Dictionary<string, List<Hero>> grouped, string title)
        {
            if (!grouped.Any())
            {
                MessageBox.Show("Нет данных для отображения",
                    "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var form = new Form
            {
                Text = title,
                Size = new Size(500, 400),
                StartPosition = FormStartPosition.CenterParent,
                MaximizeBox = false
            };

            var treeView = new TreeView
            {
                Dock = DockStyle.Fill,
                CheckBoxes = false,
                FullRowSelect = true
            };

            foreach (var group in grouped.OrderBy(g => g.Key))
            {
                var node = new TreeNode($"{group.Key} ({group.Value.Count} героев)");
                foreach (var hero in group.Value.OrderBy(h => h.Name))
                {
                    var heroNode = new TreeNode($"{hero.Name} - Сила: {hero.Strange}, HP: {hero.Hp:F1}");
                    heroNode.Tag = hero;
                    node.Nodes.Add(heroNode);
                }
                treeView.Nodes.Add(node);
            }

            var btnClose = new Button
            {
                Text = "Закрыть",
                Dock = DockStyle.Bottom,
                Height = 30
            };
            btnClose.Click += (s, e) => form.Close();

            var panel = new Panel { Dock = DockStyle.Fill };
            panel.Controls.Add(treeView);

            form.Controls.Add(panel);
            form.Controls.Add(btnClose);
            form.ShowDialog();
        }

        private void ShowHeroesList(List<Hero> heroes, string title)
        {
            if (!heroes.Any())
            {
                MessageBox.Show("Нет данных для отображения",
                    "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var form = new Form
            {
                Text = title,
                Size = new Size(500, 400),
                StartPosition = FormStartPosition.CenterParent,
                MaximizeBox = false
            };

            var dataGridView = new DataGridView
            {
                Dock = DockStyle.Fill,
                DataSource = heroes.Select(h => new
                {
                    h.Id,
                    h.Name,
                    SpeciesName = h.Species?.Name ?? "Неизвестно",
                    h.Hp,
                    h.Strange,
                    h.Genre,
                    h.TypeOfDamage
                }).ToList(),
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };

            var btnClose = new Button
            {
                Text = "Закрыть",
                Dock = DockStyle.Bottom,
                Height = 30
            };
            btnClose.Click += (s, e) => form.Close();

            var panel = new Panel { Dock = DockStyle.Fill };
            panel.Controls.Add(dataGridView);

            form.Controls.Add(panel);
            form.Controls.Add(btnClose);
            form.ShowDialog();
        }

        // Двойной клик по DataGridView
        private void MainForm_Load(object sender, EventArgs e)
        {
            dataGridView1.CellDoubleClick += DataGridView1_CellDoubleClick;
        }

        private void DataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dataGridView1.Rows[e.RowIndex].DataBoundItem != null)
            {
                try
                {
                    var heroData = dataGridView1.Rows[e.RowIndex].DataBoundItem as dynamic;
                    if (heroData != null)
                    {
                        MessageBox.Show(
                            $"Информация о герое:\n\n" +
                            $"Имя: {heroData.Name}\n" +
                            $"Раса: {heroData.SpeciesName}\n" +
                            $"HP: {heroData.Hp:F1}\n" +
                            $"Сила: {heroData.Strange}\n" +
                            $"Гендер: {heroData.Genre}\n" +
                            $"Тип урона: {heroData.TypeOfDamage}",
                            "Информация о герое",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при получении информации: {ex.Message}",
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
