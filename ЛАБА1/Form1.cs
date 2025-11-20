using BusinessLogicLayer;
using BusinessLogicLayer.Services;
using Ninject;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ЛАБА1;


namespace ЛАБА1
{
    public partial class Form1 : Form
    {
        private Logic logic;
        private int currentPage = 1;
        private int pageSize = 10;
        private int totalHeroes = 0;
        private int totalPages = 0;
        private Label lblCurrentPage;
        private Label lblTotalPages;
        private Button btnFirst;
        private Button btnPrev;
        private Button btnNext;
        private Button btnLast;
        private ComboBox cmbPageSize;

        public Form1()
        {
            InitializeComponent();
            IKernel ninjectKernel = new StandardKernel(new SimpleConfigModule());
            logic = ninjectKernel.Get<Logic>();

            InitializePaginationControls();
            RefreshHeroesList();
        }

        /// <summary>
        /// Инициализация элементов управления пагинацией
        /// </summary>
        private void InitializePaginationControls()
        {
            var paginationPanel = new Panel { Dock = DockStyle.Bottom, Height = 40 };

            var lblPageSize = new Label { Text = "На странице:", Location = new Point(10, 10), AutoSize = true };
            cmbPageSize = new ComboBox
            {
                Location = new Point(90, 7),
                Width = 60,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbPageSize.Items.AddRange(new object[] { 5, 10, 20, 50 });
            cmbPageSize.SelectedItem = pageSize;
            cmbPageSize.SelectedIndexChanged += (s, e) =>
            {
                pageSize = (int)cmbPageSize.SelectedItem;
                currentPage = 1;
                RefreshHeroesList();
            };

            btnFirst = new Button { Text = "«", Location = new Point(160, 7), Width = 30 };
            btnPrev = new Button { Text = "‹", Location = new Point(195, 7), Width = 30 };
            lblCurrentPage = new Label { Text = "1", Location = new Point(230, 10), AutoSize = true };
            lblTotalPages = new Label { Text = "/ 1", Location = new Point(245, 10), AutoSize = true };
            btnNext = new Button { Text = "›", Location = new Point(270, 7), Width = 30 };
            btnLast = new Button { Text = "»", Location = new Point(305, 7), Width = 30 };
            var btnStatistics = new Button
            {
                Text = "Статистика",
                Location = new Point(350, 7),
                Width = 80,
                Height = 25
            };
            btnStatistics.Click += btnStatistics_Click;

            btnFirst.Click += (s, e) => { currentPage = 1; RefreshHeroesList(); };
            btnPrev.Click += (s, e) => { if (currentPage > 1) { currentPage--; RefreshHeroesList(); } };
            btnNext.Click += (s, e) => { if (currentPage < totalPages) { currentPage++; RefreshHeroesList(); } };
            btnLast.Click += (s, e) => { currentPage = totalPages; RefreshHeroesList(); };

            paginationPanel.Controls.AddRange(new Control[]
            {
        lblPageSize, cmbPageSize,
        btnFirst, btnPrev, lblCurrentPage, lblTotalPages, btnNext, btnLast,
        btnStatistics
            });

            this.Controls.Add(paginationPanel);
        }
        private void btnStatistics_Click(object sender, EventArgs e)
        {
            ShowStatistics();
        }
        private void ShowStatistics()
        {
            try
            {
                IKernel ninjectKernel = new StandardKernel(new SimpleConfigModule());
                var heroService = ninjectKernel.Get<IHeroService>();

                var statistics = heroService.GetStatistics();
                DisplayStatisticsInForm(statistics);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при выводе статистики: {ex.Message}", "Ошибка",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void DisplayStatisticsInForm(HeroStatistics stats)
        {
            var statsForm = new Form
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
            sb.AppendLine($"Минимальное HP: {stats.MinHp}");
            sb.AppendLine();

            sb.AppendLine("СТАТИСТИКА ПО РАСАМ:");
            foreach (var stat in stats.SpeciesStats)
            {
                sb.AppendLine($"  {stat.Species}:");
                sb.AppendLine($"    Количество: {stat.Count} героев");
                sb.AppendLine($"    Средняя сила: {stat.AvgStrength:F1}");
                sb.AppendLine($"    Среднее HP: {stat.AvgHp:F1}");
            }
            sb.AppendLine();

            sb.AppendLine("СТАТИСТИКА ПО ТИПАМ УРОНА:");
            foreach (var stat in stats.DamageTypeStats)
            {
                sb.AppendLine($"  {stat.DamageType}:");
                sb.AppendLine($"    Количество: {stat.Count} героев");
                sb.AppendLine($"    Общая сила: {stat.TotalStrength}");
            }
            sb.AppendLine();

            sb.AppendLine("СТАТИСТИКА ПО ГЕНДЕРАМ:");
            foreach (var stat in stats.GenderStats)
            {
                sb.AppendLine($"  {stat.Gender}: {stat.Count} героев ({stat.Percentage:F1}%)");
            }
            sb.AppendLine();

            sb.AppendLine($"ГЕРОИ С НИЗКИМ HP (<50): {stats.LowHpHeroes.Count}");
            foreach (var hero in stats.LowHpHeroes)
            {
                sb.AppendLine($"  {hero.Name} - {hero.Hp} HP ({hero.Species?.Name})");
            }
            sb.AppendLine();
            sb.AppendLine("═══════════════════════════════════════");

            textBox.Text = sb.ToString();
            statsForm.Controls.Add(textBox);
            statsForm.ShowDialog();
        }
        /// <summary>
        /// Обновление списка героев с учетом пагинации
        /// </summary>
        public void RefreshHeroesList()
        {
            try
            {
                totalHeroes = logic.GetListHeros().Count;
                totalPages = (int)Math.Ceiling((double)totalHeroes / pageSize);

                if (currentPage > totalPages && totalPages > 0)
                    currentPage = totalPages;
                else if (totalPages == 0)
                    currentPage = 1;

                var allHeroes = logic.GetListHeros();
                var pagedHeroes = allHeroes
                    .Skip((currentPage - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                var displayData = pagedHeroes.Select(h => new
                {
                    h.Id,
                    h.Name,
                    SpeciesName = h.Species.Name,
                    h.Hp,
                    h.Strange,
                    h.Genre,
                    h.TypeOfDamage
                }).ToList();

                dataGridView1.DataSource = displayData;

                UpdatePaginationInfo();
                UpdateStatusBar();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обновлении списка героев: {ex.Message}");
            }
        }

        /// <summary>
        /// Обновление информации о пагинации
        /// </summary>
        private void UpdatePaginationInfo()
        {
            if (lblCurrentPage != null && lblTotalPages != null)
            {
                lblCurrentPage.Text = currentPage.ToString();
                lblTotalPages.Text = $"/ {totalPages}";

                btnFirst.Enabled = currentPage > 1;
                btnPrev.Enabled = currentPage > 1;
                btnNext.Enabled = currentPage < totalPages;
                btnLast.Enabled = currentPage < totalPages;
            }
        }

        /// <summary>
        /// Обновление статусной строки
        /// </summary>
        private void UpdateStatusBar()
        {
            if (statusStrip1.Items.Count > 0)
            {
                var statusLabel = statusStrip1.Items[0] as ToolStripStatusLabel;
                if (statusLabel != null)
                {
                    statusLabel.Text = $"Всего героев: {totalHeroes} | Страница: {currentPage} из {totalPages}";
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dataGridView1.Columns.Clear();
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Id",
                HeaderText = "ID",
                Width = 40
            });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Name",
                HeaderText = "Имя героя",
                Width = 120
            });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "SpeciesName", 
                HeaderText = "Раса",
                Width = 100
            });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Hp",
                HeaderText = "HP",
                Width = 60
            });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Strange",
                HeaderText = "Сила",
                Width = 50
            });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Genre",
                HeaderText = "Гендер",
                Width = 70
            });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "TypeOfDamage",
                HeaderText = "Тип урона",
                Width = 120
            });

            RefreshHeroesList();
        }
        private void добавитьГерояToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var form = new AddHeroForm())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    logic.CreateHero(form.HeroName, form.HeroSpeciesId, form.HeroGenre,
                                   form.HeroStrange, form.HeroDamageType, form.HeroHp);
                    RefreshHeroesList();
                    Program.RefreshFormData();
                }
            }
        }

        private void удалитьГерояToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow?.DataBoundItem is Hero selectedHero)
            {
                var result = MessageBox.Show($"Удалить героя '{selectedHero.Name}'?",
                                           "Подтверждение удаления",
                                           MessageBoxButtons.YesNo,
                                           MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    logic.KillHero(selectedHero.Id);
                    RefreshHeroesList();
                    Program.RefreshFormData();
                }
            }
        }

        private void нанестиУронToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow?.DataBoundItem is Hero selectedHero)
            {
                using (var form = new HitHeroForm(selectedHero))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        logic.HitHero(selectedHero.Id, form.DamageAmount);
                        RefreshHeroesList();
                        Program.RefreshFormData();
                    }
                }
            }
        }

        private void обновитьСписокToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RefreshHeroesList();
        }
        private void показатьВсегоГероевToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowAllHeroes();
        }

        private void найтиПоИмениToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FindByName();
        }

        private void группировкаПоРасамToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowBySpecies();
        }

        private void группировкаПоТипуУронаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowByDamageType();
        }

        private void раненыеГероиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowWoundedHeroes();
        }

        private void топ3СильнейшихToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowStrongestHeroes();
        }

        private void показатьВсеРасыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowAllSpecies();
        }

        /// <summary>
        /// Показать всех героев
        /// </summary>
        private void ShowAllHeroes()
        {
            currentPage = 1;
            pageSize = 1000; // Большое число чтобы показать всех
            RefreshHeroesList();
            MessageBox.Show("Показаны все герои", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
            pageSize = 10; // Возвращаем нормальный размер страницы
            cmbPageSize.SelectedItem = 10;
        }

        /// <summary>
        /// Поиск героя по имени
        /// </summary>
        private void FindByName()
        {
            var searchForm = new Form
            {
                Text = "Поиск героя",
                Size = new Size(300, 150),
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterParent
            };

            var lblName = new Label { Text = "Имя:", Location = new Point(10, 20), Width = 100 };
            var txtName = new TextBox { Location = new Point(80, 17), Width = 150 };
            var btnSearch = new Button { Text = "Найти", Location = new Point(80, 50), Width = 80 };
            var btnCancel = new Button { Text = "Отмена", Location = new Point(170, 50), Width = 80 };

            btnSearch.Click += (s, e) =>
            {
                var heroes = logic.FindHeroesByName(txtName.Text);
                if (heroes.Count > 0)
                {
                    var resultForm = new Form
                    {
                        Text = "Результаты поиска",
                        Size = new Size(400, 300),
                        StartPosition = FormStartPosition.CenterParent
                    };

                    var grid = new DataGridView
                    {
                        Dock = DockStyle.Fill,
                        DataSource = heroes,
                        ReadOnly = true
                    };

                    resultForm.Controls.Add(grid);
                    resultForm.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Герои не найдены", "Результат поиска", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                searchForm.Close();
            };

            btnCancel.Click += (s, e) => searchForm.Close();

            searchForm.Controls.AddRange(new Control[] { lblName, txtName, btnSearch, btnCancel });
            searchForm.ShowDialog();
        }

        /// <summary>
        /// Группировка по расам
        /// </summary>
        private void ShowBySpecies()
        {
            var groupedHeroes = logic.GroupHeroesBySpecies();
            var resultForm = new Form
            {
                Text = "Герои по расам",
                Size = new Size(500, 400),
                StartPosition = FormStartPosition.CenterParent
            };

            var treeView = new TreeView { Dock = DockStyle.Fill };

            foreach (var speciesGroup in groupedHeroes)
            {
                var speciesNode = new TreeNode(speciesGroup.Key);
                foreach (var hero in speciesGroup.Value)
                {
                    speciesNode.Nodes.Add($"{hero.Name} - Сила: {hero.Strange}, HP: {hero.Hp}");
                }
                treeView.Nodes.Add(speciesNode);
            }

            resultForm.Controls.Add(treeView);
            resultForm.ShowDialog();
        }

        /// <summary>
        /// Группировка по типу урона
        /// </summary>
        private void ShowByDamageType()
        {
            var groupedHeroes = logic.GroupHeroesByDamageType();
            var resultForm = new Form
            {
                Text = "Герои по типу урона",
                Size = new Size(500, 400),
                StartPosition = FormStartPosition.CenterParent
            };

            var treeView = new TreeView { Dock = DockStyle.Fill };

            foreach (var damageGroup in groupedHeroes)
            {
                var damageNode = new TreeNode(damageGroup.Key);
                foreach (var hero in damageGroup.Value)
                {
                    var speciesName = hero.Species?.Name ?? "Неизвестно";
                    damageNode.Nodes.Add($"{hero.Name} ({speciesName}) - HP: {hero.Hp}");
                }
                treeView.Nodes.Add(damageNode);
            }

            resultForm.Controls.Add(treeView);
            resultForm.ShowDialog();
        }

        /// <summary>
        /// Показать раненых героев
        /// </summary>
        private void ShowWoundedHeroes()
        {
            var wounded = logic.GetHeroesWithLowHp(50);
            if (wounded.Count > 0)
            {
                var resultForm = new Form
                {
                    Text = "Раненые герои (HP < 50)",
                    Size = new Size(400, 300),
                    StartPosition = FormStartPosition.CenterParent
                };

                var grid = new DataGridView
                {
                    Dock = DockStyle.Fill,
                    DataSource = wounded,
                    ReadOnly = true
                };

                resultForm.Controls.Add(grid);
                resultForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Раненых героев не найдено", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// Показать топ-3 сильнейших героя
        /// </summary>
        private void ShowStrongestHeroes()
        {
            var strongest = logic.GetStrongestHeroes(3);
            var resultForm = new Form
            {
                Text = "Топ-3 самых сильных героя",
                Size = new Size(400, 200),
                StartPosition = FormStartPosition.CenterParent
            };

            var listBox = new ListBox { Dock = DockStyle.Fill };
            foreach (var hero in strongest)
            {
                var speciesName = hero.Species?.Name ?? "Неизвестно";
                listBox.Items.Add($"{hero.Name} - {speciesName} - Сила: {hero.Strange}, HP: {hero.Hp}");
            }

            resultForm.Controls.Add(listBox);
            resultForm.ShowDialog();
        }

        /// <summary>
        /// Показать все расы
        /// </summary>
        private void ShowAllSpecies()
        {
            var speciesList = logic.GetAllSpecies();
            var resultForm = new Form
            {
                Text = "Все расы",
                Size = new Size(400, 300),
                StartPosition = FormStartPosition.CenterParent
            };

            var grid = new DataGridView
            {
                Dock = DockStyle.Fill,
                DataSource = speciesList,
                ReadOnly = true
            };

            resultForm.Controls.Add(grid);
            resultForm.ShowDialog();
        }
    }
}