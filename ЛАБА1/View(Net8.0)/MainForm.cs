// View/MainForm.cs
using Presenter;
using Shared;
using Shared.Domain;
using Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace View
{
    public partial class MainForm : Form, IView
    {
        private MainPresenter _presenter;
        private DataGridView dataGridView1;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel statusLabel;
        private MenuStrip menuStrip1;
        private ToolStrip toolStrip1;
        private Panel paginationPanel;
        private Label lblPageInfo;
        private ComboBox cmbPageSize;
        private Button btnFirst, btnPrev, btnNext, btnLast;
        private TextBox txtSearch;

        // Реализация событий интерфейса IView
        public event Action<HeroAddedEventArgs> HeroAdded;
        public event Action<HeroDeletedEventArgs> HeroDeleted;
        public event Action<HeroDamagedEventArgs> HeroDamaged;
        public event Action<HeroSearchEventArgs> HeroSearch;
        public event Action<PageChangedEventArgs> PageChanged;
        public event Action RefreshRequested;
        public event Action<SpeciesAddedEventArgs> SpeciesAdded;
        public event Action<SpeciesDeletedEventArgs> SpeciesDeleted;
        public event Action<SpeciesUpdatedEventArgs> SpeciesUpdated;

        public MainForm()
        {
            InitializeForm();
            InitializeMenu();
            InitializeToolbar();
            InitializePagination();
            InitializeSearchBox();

            // Создаем презентер и передаем ему себя (View)
            _presenter = new MainPresenter(this);

            // Начальная загрузка данных
            RefreshRequested?.Invoke();
        }

        private void InitializeForm()
        {
            this.Text = "Герои - Управление персонажами (MVP Architecture)";
            this.Size = new Size(1000, 650);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // Main Layout Panel
            var mainPanel = new Panel { Dock = DockStyle.Fill };

            // DataGridView - ВАЖНО: AutoGenerateColumns = false
            dataGridView1 = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AutoGenerateColumns = false, // ОТКЛЮЧАЕМ автосоздание колонок!
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                BackgroundColor = SystemColors.ControlLightLight,
                BorderStyle = BorderStyle.Fixed3D,
                CellBorderStyle = DataGridViewCellBorderStyle.Single,
                RowHeadersVisible = false
            };

            // Настройка колонок DataGridView
            ConfigureDataGridViewColumns();

            mainPanel.Controls.Add(dataGridView1);
            this.Controls.Add(mainPanel);

            // Status Bar
            statusStrip1 = new StatusStrip();
            statusLabel = new ToolStripStatusLabel
            {
                Text = "Готово",
                Spring = true,
                TextAlign = ContentAlignment.MiddleLeft
            };
            statusStrip1.Items.Add(statusLabel);
            this.Controls.Add(statusStrip1);
        }

        private void ConfigureDataGridViewColumns()
        {
            dataGridView1.Columns.Clear();

            // Колонка ID
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colId",
                HeaderText = "ID",
                DataPropertyName = "Id", // Связь с полем Id
                Width = 50,
                ReadOnly = true
            });

            // Колонка Имя
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colName",
                HeaderText = "Имя героя",
                DataPropertyName = "Name", // Связь с полем Name
                Width = 150,
                ReadOnly = true
            });

            // Колонка Раса
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colSpecies",
                HeaderText = "Раса",
                DataPropertyName = "SpeciesName", // Связь с Species.Name через SpeciesName
                Width = 100,
                ReadOnly = true
            });

            // Колонка HP
            var hpColumn = new DataGridViewTextBoxColumn
            {
                Name = "colHp",
                HeaderText = "HP",
                DataPropertyName = "Hp", // Связь с полем Hp
                Width = 80,
                ReadOnly = true
            };
            dataGridView1.Columns.Add(hpColumn);

            // Колонка Сила
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colStrength",
                HeaderText = "Сила",
                DataPropertyName = "Strange", // Связь с полем Strange
                Width = 60,
                ReadOnly = true
            });

            // Колонка Гендер
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colGenre",
                HeaderText = "Гендер",
                DataPropertyName = "Genre", // Связь с полем Genre
                Width = 80,
                ReadOnly = true
            });

            // Колонка Тип урона
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colDamageType",
                HeaderText = "Тип урона",
                DataPropertyName = "TypeOfDamage", // Связь с полем TypeOfDamage
                Width = 120,
                ReadOnly = true
            });

            // Обработчик форматирования ячеек
            dataGridView1.CellFormatting += DataGridView1_CellFormatting;
            // Двойной клик по строке
            dataGridView1.CellDoubleClick += DataGridView1_CellDoubleClick;
            // Выделение строки
            dataGridView1.SelectionChanged += DataGridView1_SelectionChanged;
        }

        private void InitializeMenu()
        {
            menuStrip1 = new MenuStrip();

            // Меню Файл
            var fileMenu = new ToolStripMenuItem("Файл");

            var exitItem = new ToolStripMenuItem("Выход");
            exitItem.Click += (s, e) => Application.Exit();

            fileMenu.DropDownItems.Add(new ToolStripSeparator());
            fileMenu.DropDownItems.Add(exitItem);

            // Меню Герои
            var heroesMenu = new ToolStripMenuItem("Герои");

            var addHeroItem = new ToolStripMenuItem("Добавить героя");
            addHeroItem.Click += BtnAddHero_Click;

            var deleteHeroItem = new ToolStripMenuItem("Удалить героя");
            deleteHeroItem.Click += BtnDeleteHero_Click;

            var hitHeroItem = new ToolStripMenuItem("Нанести урон");
            hitHeroItem.Click += BtnHitHero_Click;

            var refreshItem = new ToolStripMenuItem("Обновить список");
            refreshItem.Click += BtnRefresh_Click;

            heroesMenu.DropDownItems.Add(addHeroItem);
            heroesMenu.DropDownItems.Add(deleteHeroItem);
            heroesMenu.DropDownItems.Add(hitHeroItem);
            heroesMenu.DropDownItems.Add(new ToolStripSeparator());
            heroesMenu.DropDownItems.Add(refreshItem);

            // Меню Расы
            var speciesMenu = new ToolStripMenuItem("Расы");

            var addSpeciesItem = new ToolStripMenuItem("Добавить расу");
            addSpeciesItem.Click += BtnAddSpecies_Click;

            var showSpeciesItem = new ToolStripMenuItem("Показать все расы");
            showSpeciesItem.Click += BtnShowSpecies_Click;

            var editSpeciesItem = new ToolStripMenuItem("Редактировать расу");
            editSpeciesItem.Click += BtnEditSpecies_Click;

            var deleteSpeciesItem = new ToolStripMenuItem("Удалить расу");
            deleteSpeciesItem.Click += BtnDeleteSpecies_Click;

            speciesMenu.DropDownItems.Add(addSpeciesItem);
            speciesMenu.DropDownItems.Add(showSpeciesItem);
            speciesMenu.DropDownItems.Add(new ToolStripSeparator());
            speciesMenu.DropDownItems.Add(editSpeciesItem);
            speciesMenu.DropDownItems.Add(deleteSpeciesItem);

            // Меню Вид
            var viewMenu = new ToolStripMenuItem("Вид");

            var statisticsItem = new ToolStripMenuItem("Статистика");
            statisticsItem.Click += BtnStatistics_Click;

            var groupBySpeciesItem = new ToolStripMenuItem("Группировка по расам");
            groupBySpeciesItem.Click += BtnGroupBySpecies_Click;

            var groupByDamageItem = new ToolStripMenuItem("Группировка по типу урона");
            groupByDamageItem.Click += BtnGroupByDamage_Click;

            var showWoundedItem = new ToolStripMenuItem("Раненые герои (HP < 50)");
            showWoundedItem.Click += BtnShowWounded_Click;

            var showStrongestItem = new ToolStripMenuItem("Топ-3 сильнейших");
            showStrongestItem.Click += BtnShowStrongest_Click;

            viewMenu.DropDownItems.Add(statisticsItem);
            viewMenu.DropDownItems.Add(new ToolStripSeparator());
            viewMenu.DropDownItems.Add(groupBySpeciesItem);
            viewMenu.DropDownItems.Add(groupByDamageItem);
            viewMenu.DropDownItems.Add(showWoundedItem);
            viewMenu.DropDownItems.Add(showStrongestItem);

            // Добавляем меню
            menuStrip1.Items.Add(fileMenu);
            menuStrip1.Items.Add(heroesMenu);
            menuStrip1.Items.Add(speciesMenu);
            menuStrip1.Items.Add(viewMenu);

            this.MainMenuStrip = menuStrip1;
            this.Controls.Add(menuStrip1);
        }

        private void InitializeToolbar()
        {
            toolStrip1 = new ToolStrip();
            toolStrip1.Dock = DockStyle.Top;

            // Кнопка Добавить героя
            var btnAdd = new ToolStripButton("➕ Добавить героя");
            btnAdd.Click += BtnAddHero_Click;
            toolStrip1.Items.Add(btnAdd);

            // Кнопка Удалить
            var btnDelete = new ToolStripButton("❌ Удалить");
            btnDelete.Click += BtnDeleteHero_Click;
            toolStrip1.Items.Add(btnDelete);

            // Кнопка Нанести урон
            var btnHit = new ToolStripButton("⚔️ Нанести урон");
            btnHit.Click += BtnHitHero_Click;
            toolStrip1.Items.Add(btnHit);

            toolStrip1.Items.Add(new ToolStripSeparator());

            // Кнопка Обновить
            var btnRefresh = new ToolStripButton("🔄 Обновить");
            btnRefresh.Click += BtnRefresh_Click;
            toolStrip1.Items.Add(btnRefresh);

            toolStrip1.Items.Add(new ToolStripSeparator());

            // Кнопка Статистика
            var btnStats = new ToolStripButton("📊 Статистика");
            btnStats.Click += BtnStatistics_Click;
            toolStrip1.Items.Add(btnStats);

            this.Controls.Add(toolStrip1);
        }

        private void InitializePagination()
        {
            paginationPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 40,
                BackColor = Color.LightGray
            };

            int x = 10;

            // Метка "На странице"
            var lblPageSize = new Label
            {
                Text = "На странице:",
                Location = new Point(x, 10),
                AutoSize = true
            };
            paginationPanel.Controls.Add(lblPageSize);
            x += 80;

            // Комбобокс выбора размера страницы
            cmbPageSize = new ComboBox
            {
                Location = new Point(x, 7),
                Width = 60,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbPageSize.Items.AddRange(new object[] { 5, 10, 20, 50, 100 });
            cmbPageSize.SelectedItem = 10;
            cmbPageSize.SelectedIndexChanged += CmbPageSize_SelectedIndexChanged;
            paginationPanel.Controls.Add(cmbPageSize);
            x += 70;

            // Кнопка "Первая"
            btnFirst = new Button
            {
                Text = "⏮️ Первая",
                Location = new Point(x, 7),
                Width = 80,
                Enabled = false
            };
            btnFirst.Click += BtnFirst_Click;
            paginationPanel.Controls.Add(btnFirst);
            x += 85;

            // Кнопка "Предыдущая"
            btnPrev = new Button
            {
                Text = "◀️ Назад",
                Location = new Point(x, 7),
                Width = 80,
                Enabled = false
            };
            btnPrev.Click += BtnPrev_Click;
            paginationPanel.Controls.Add(btnPrev);
            x += 85;

            // Информация о странице
            lblPageInfo = new Label
            {
                Text = "Страница 1 из 1",
                Location = new Point(x, 10),
                AutoSize = true,
                Font = new Font(Font, FontStyle.Bold)
            };
            paginationPanel.Controls.Add(lblPageInfo);
            x += 100;

            // Кнопка "Следующая"
            btnNext = new Button
            {
                Text = "Вперёд ▶️",
                Location = new Point(x, 7),
                Width = 80,
                Enabled = false
            };
            btnNext.Click += BtnNext_Click;
            paginationPanel.Controls.Add(btnNext);
            x += 85;

            // Кнопка "Последняя"
            btnLast = new Button
            {
                Text = "Последняя ⏭️",
                Location = new Point(x, 7),
                Width = 80,
                Enabled = false
            };
            btnLast.Click += BtnLast_Click;
            paginationPanel.Controls.Add(btnLast);

            this.Controls.Add(paginationPanel);
        }

        private void InitializeSearchBox()
        {
            var searchPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 35,
                BackColor = Color.WhiteSmoke
            };

            var lblSearch = new Label
            {
                Text = "Поиск:",
                Location = new Point(10, 8),
                AutoSize = true
            };
            searchPanel.Controls.Add(lblSearch);

            txtSearch = new TextBox
            {
                Location = new Point(60, 5),
                Width = 200
            };
            txtSearch.KeyPress += TxtSearch_KeyPress;
            searchPanel.Controls.Add(txtSearch);

            var btnSearch = new Button
            {
                Text = "🔍 Найти",
                Location = new Point(270, 5),
                Width = 80
            };
            btnSearch.Click += BtnSearch_Click;
            searchPanel.Controls.Add(btnSearch);

            var btnClearSearch = new Button
            {
                Text = "Очистить",
                Location = new Point(360, 5),
                Width = 80
            };
            btnClearSearch.Click += BtnClearSearch_Click;
            searchPanel.Controls.Add(btnClearSearch);

            this.Controls.Add(searchPanel);
        }

        #region Обработчики событий UI

        private void BtnAddHero_Click(object sender, EventArgs e)
        {
            using (var form = new AddHeroForm())
            {
                // В реальном приложении нужно загрузить список рас через Presenter
                // form.SetSpeciesList(speciesData);

                form.HeroAdded += (heroForm) =>
                {
                    // Генерируем событие для Presenter
                    HeroAdded?.Invoke(new HeroAddedEventArgs
                    {
                        Name = heroForm.HeroName,
                        SpeciesId = heroForm.HeroSpeciesId,
                        Genre = heroForm.HeroGenre,
                        Strange = heroForm.HeroStrange,
                        DamageType = heroForm.HeroDamageType,
                        Hp = heroForm.HeroHp
                    });
                };

                form.ShowDialog();
            }
        }

        private void BtnDeleteHero_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow?.DataBoundItem != null)
            {
                dynamic heroData = dataGridView1.CurrentRow.DataBoundItem;

                var result = MessageBox.Show(
                    $"Вы действительно хотите удалить героя '{heroData.Name}'?\n" +
                    "Это действие нельзя отменить!",
                    "Подтверждение удаления",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    // Генерируем событие для Presenter
                    HeroDeleted?.Invoke(new HeroDeletedEventArgs
                    {
                        HeroId = heroData.Id
                    });
                }
            }
            else
            {
                ShowMessage("Выберите героя для удаления", "Информация");
            }
        }

        private void BtnHitHero_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow?.DataBoundItem != null)
            {
                dynamic heroData = dataGridView1.CurrentRow.DataBoundItem;

                using (var form = new HitHeroForm(heroData))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        // Генерируем событие для Presenter
                        HeroDamaged?.Invoke(new HeroDamagedEventArgs
                        {
                            HeroId = heroData.Id,
                            Damage = form.DamageAmount
                        });
                    }
                }
            }
            else
            {
                ShowMessage("Выберите героя для нанесения урона", "Информация");
            }
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            RefreshRequested?.Invoke();
            txtSearch.Clear();
        }

        private void BtnAddSpecies_Click(object sender, EventArgs e)
        {
            using (var form = new AddSpeciesForm())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    // Генерируем событие для Presenter
                    SpeciesAdded?.Invoke(new SpeciesAddedEventArgs
                    {
                        Name = form.SpeciesName,
                        Description = form.SpeciesDescription
                    });
                }
            }
        }

        private void BtnShowSpecies_Click(object sender, EventArgs e)
        {
            // Запрашиваем у Presenter список рас
            // В реальном приложении нужно реализовать метод в Presenter
            ShowMessage("Функция показа всех рас будет реализована", "Информация");
        }

        private void BtnEditSpecies_Click(object sender, EventArgs e)
        {
            ShowMessage("Функция редактирования рас будет реализована", "Информация");
        }

        private void BtnDeleteSpecies_Click(object sender, EventArgs e)
        {
            ShowMessage("Функция удаления рас будет реализована", "Информация");
        }

        private void BtnStatistics_Click(object sender, EventArgs e)
        {
            // Запрашиваем у Presenter статистику
            // В реальном приложении нужно реализовать метод в Presenter
            ShowMessage("Функция статистики будет реализована", "Информация");
        }

        private void BtnGroupBySpecies_Click(object sender, EventArgs e)
        {
            ShowMessage("Группировка по расам будет реализована", "Информация");
        }

        private void BtnGroupByDamage_Click(object sender, EventArgs e)
        {
            ShowMessage("Группировка по типу урона будет реализована", "Информация");
        }

        private void BtnShowWounded_Click(object sender, EventArgs e)
        {
            ShowMessage("Список раненых героев будет показан", "Информация");
        }

        private void BtnShowStrongest_Click(object sender, EventArgs e)
        {
            ShowMessage("Топ-3 сильнейших героев будет показан", "Информация");
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                // Генерируем событие поиска для Presenter
                HeroSearch?.Invoke(new HeroSearchEventArgs
                {
                    SearchTerm = txtSearch.Text.Trim()
                });
            }
        }

        private void BtnClearSearch_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            RefreshRequested?.Invoke();
        }

        private void TxtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                BtnSearch_Click(sender, e);
                e.Handled = true;
            }
        }

        private void CmbPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbPageSize.SelectedItem != null)
            {
                // Генерируем событие смены страницы для Presenter
                PageChanged?.Invoke(new PageChangedEventArgs
                {
                    PageSize = (int)cmbPageSize.SelectedItem,
                    PageNumber = 1 // Сбрасываем на первую страницу
                });
            }
        }

        private void BtnFirst_Click(object sender, EventArgs e)
        {
            PageChanged?.Invoke(new PageChangedEventArgs
            {
                PageSize = _pageSize,
                PageNumber = 1
            });
        }

        private void BtnPrev_Click(object sender, EventArgs e)
        {
            if (_currentPage > 1)
            {
                PageChanged?.Invoke(new PageChangedEventArgs
                {
                    PageSize = _pageSize,
                    PageNumber = _currentPage - 1
                });
            }
        }

        private void BtnNext_Click(object sender, EventArgs e)
        {
            PageChanged?.Invoke(new PageChangedEventArgs
            {
                PageSize = _pageSize,
                PageNumber = _currentPage + 1
            });
        }

        private void BtnLast_Click(object sender, EventArgs e)
        {
            PageChanged?.Invoke(new PageChangedEventArgs
            {
                PageSize = _pageSize,
                PageNumber = _totalPages
            });
        }

        #endregion

        #region Реализация интерфейса IView

        private int _currentPage = 1;
        private int _pageSize = 10;
        private int _totalPages = 1;

        public void RefreshHeroesList(List<Hero> heroes)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<List<Hero>>(RefreshHeroesList), heroes);
                return;
            }

            try
            {
                // Преобразуем героев в анонимный тип для DataGridView
                // ВАЖНО: свойства должны совпадать с DataPropertyName колонок
                var displayData = heroes.Select(h => new
                {
                    Id = h.Id,
                    Name = h.Name,
                    SpeciesName = h.Species?.Name ?? "Неизвестно", // Используем SpeciesName вместо Species
                    Hp = h.Hp,
                    Strange = h.Strange, // Используем Strange вместо Strength
                    Genre = h.Genre,
                    TypeOfDamage = h.TypeOfDamage // Используем TypeOfDamage вместо DamageType
                }).ToList();

                dataGridView1.DataSource = null; // Очищаем данные
                dataGridView1.DataSource = displayData; // Устанавливаем новые данные

                // Обновляем статус
                UpdateStatusBar($"Показано {heroes.Count} из {_totalItems} героев");
            }
            catch (Exception ex)
            {
                ShowError($"Ошибка при обновлении списка героев: {ex.Message}");
                Console.WriteLine($"Ошибка в RefreshHeroesList: {ex}");
            }
        }

        public void UpdateStatusBar(string status)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<string>(UpdateStatusBar), status);
                return;
            }

            statusLabel.Text = status;
        }

        public void ShowMessage(string message, string title = "Информация")
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<string, string>(ShowMessage), message, title);
                return;
            }

            MessageBox.Show(this, message, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void ShowError(string error, string title = "Ошибка")
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<string, string>(ShowError), error, title);
                return;
            }

            MessageBox.Show(this, error, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public void SetPaginationInfo(int currentPage, int totalPages, int totalItems)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<int, int, int>(SetPaginationInfo), currentPage, totalPages, totalItems);
                return;
            }

            _currentPage = currentPage;
            _totalPages = totalPages;
            _totalItems = totalItems;

            lblPageInfo.Text = $"Страница {currentPage} из {totalPages} (Всего: {totalItems})";

            btnFirst.Enabled = currentPage > 1;
            btnPrev.Enabled = currentPage > 1;
            btnNext.Enabled = currentPage < totalPages;
            btnLast.Enabled = currentPage < totalPages;

            // Обновляем выбранный размер страницы
            if (cmbPageSize.SelectedItem == null || (int)cmbPageSize.SelectedItem != _pageSize)
            {
                cmbPageSize.SelectedItem = _pageSize;
            }
        }

        public void ShowHeroDetails(Hero hero)
        {
            ShowMessage(
                $"Детальная информация о герое:\n\n" +
                $"🏷️ Имя: {hero.Name}\n" +
                $"👥 Раса: {hero.Species?.Name ?? "Неизвестно"}\n" +
                $"❤️ HP: {hero.Hp:F1}\n" +
                $"💪 Сила: {hero.Strange}\n" +
                $"👤 Гендер: {hero.Genre}\n" +
                $"⚔️ Тип урона: {hero.TypeOfDamage}\n" +
                $"🆔 ID: {hero.Id}",
                $"Герой: {hero.Name}");
        }

        public void ShowStatistics(object statistics)
        {
            // Временная реализация - в реальном приложении будет отдельная форма
            ShowMessage("Статистика будет отображена в отдельном окне", "Статистика");
        }

        public void ShowGroupedHeroes(Dictionary<string, List<Hero>> grouped, string title)
        {
            // Временная реализация
            var message = $"{title}:\n\n";
            foreach (var group in grouped)
            {
                message += $"{group.Key} ({group.Value.Count} героев):\n";
                foreach (var hero in group.Value.Take(3))
                {
                    message += $"  • {hero.Name} (HP: {hero.Hp:F1}, Сила: {hero.Strange})\n";
                }
                if (group.Value.Count > 3)
                    message += $"  ... и еще {group.Value.Count - 3} героев\n";
                message += "\n";
            }
            ShowMessage(message, title);
        }

        public void ShowSpeciesList(List<Species> species, Action<Species> onSelected = null)
        {
            // Временная реализация
            var message = "Список рас:\n\n";
            foreach (var s in species)
            {
                message += $"🟢 {s.Name} (ID: {s.Id})\n";
                if (!string.IsNullOrEmpty(s.Description))
                    message += $"   Описание: {s.Description}\n";
                message += "\n";
            }
            ShowMessage(message, "Все расы");
        }

        public void ShowHeroSelection(List<Hero> heroes, Action<Hero> onSelected = null)
        {
            // Временная реализация
            var message = "Выберите героя:\n\n";
            for (int i = 0; i < heroes.Count; i++)
            {
                var hero = heroes[i];
                message += $"{i + 1}. {hero.Name} (HP: {hero.Hp:F1}, Сила: {hero.Strange})\n";
            }
            ShowMessage(message, "Выбор героя");
        }

        #endregion

        #region Обработчики DataGridView

        private void DataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 &&
                dataGridView1.Columns[e.ColumnIndex].Name == "colHp")
            {
                var row = dataGridView1.Rows[e.RowIndex];
                if (row.DataBoundItem != null)
                {
                    dynamic heroData = row.DataBoundItem;
                    double hp = heroData.Hp;

                    // Просто показываем число без цветов и галочек
                    if (hp <= 0)
                    {
                        e.Value = "0.0 (мертв)";
                        e.CellStyle.ForeColor = Color.Gray;
                    }
                    else
                    {
                        e.Value = $"{hp:F1}";
                        e.CellStyle.ForeColor = Color.Black;
                    }
                }
            }
        }

        private void DataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dataGridView1.Rows[e.RowIndex].DataBoundItem != null)
            {
                dynamic heroData = dataGridView1.Rows[e.RowIndex].DataBoundItem;

                ShowMessage(
                    $"Краткая информация:\n\n" +
                    $"🏷️ Имя: {heroData.Name}\n" +
                    $"👥 Раса: {heroData.SpeciesName}\n" +
                    $"❤️ HP: {heroData.Hp}\n" +
                    $"💪 Сила: {heroData.Strange}\n" +
                    $"👤 Гендер: {heroData.Genre}\n" +
                    $"⚔️ Тип: {heroData.TypeOfDamage}",
                    $"Герой: {heroData.Name}");
            }
        }

        private void DataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow?.DataBoundItem != null)
            {
                dynamic heroData = dataGridView1.CurrentRow.DataBoundItem;
                UpdateStatusBar($"Выбран: {heroData.Name} | HP: {heroData.Hp} | Сила: {heroData.Strange} | Тип: {heroData.TypeOfDamage}");
            }
        }

        #endregion

        private int _totalItems = 0;

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            _presenter?.Dispose();
            base.OnFormClosed(e);
        }
    }
}