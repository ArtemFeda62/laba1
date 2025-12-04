using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Windows.Forms;
using Shared.Dtos;
using Shared.Interfaces;

namespace View
{
    public partial class MainForm : Form, IMainView
    {
        // События
        public event EventHandler LoadHeroes;
        public event EventHandler AddHero;
        public event EventHandler DeleteHero;
        public event EventHandler<int> HeroSelected;
        public event EventHandler<double> HeroDamaged;
        public event EventHandler ShowStatistics;
        public event EventHandler<int> PageChanged;
        public event EventHandler<int> PageSizeChanged;
        public event EventHandler FindHeroes;
        public event EventHandler ShowAllSpecies;

        // Элементы управления
        private DataGridView dataGridView;
        private MenuStrip menuStrip;
        private StatusStrip statusStrip;
        private Panel paginationPanel;
        private Panel searchPanel;
        private TextBox txtSearch;
        private Button btnSearch;
        private ComboBox cmbPageSize;
        private Label lblCurrentPage;
        private Label lblTotalPages;
        private Button btnFirst;
        private Button btnPrev;
        private Button btnNext;
        private Button btnLast;
        private Button btnStatistics;
        private Button btnAddHero;
        private Button btnDeleteHero;
        private Button btnHitHero;

        // Свойства
        public int SelectedHeroId
        {
            get
            {
                if (dataGridView.CurrentRow?.DataBoundItem is HeroDto hero)
                    return hero.Id;
                return -1;
            }
        }

        public string SearchName => txtSearch.Text;
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public MainForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            // Основные настройки формы
            this.Text = "Hero Management System - MVP Architecture";
            this.Size = new Size(900, 600);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Панель поиска
            searchPanel = new Panel { Dock = DockStyle.Top, Height = 40 };
            var lblSearch = new Label { Text = "Поиск:", Location = new Point(10, 10), AutoSize = true };
            txtSearch = new TextBox { Location = new Point(60, 7), Width = 150 };
            btnSearch = new Button { Text = "Найти", Location = new Point(220, 7), Width = 60 };
            btnSearch.Click += (s, e) => FindHeroes?.Invoke(this, EventArgs.Empty);

            searchPanel.Controls.AddRange(new Control[] { lblSearch, txtSearch, btnSearch });

            // DataGridView
            dataGridView = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false
            };

            dataGridView.SelectionChanged += (s, e) =>
            {
                if (dataGridView.CurrentRow?.DataBoundItem is HeroDto hero)
                {
                    HeroSelected?.Invoke(this, hero.Id);
                }
            };

            // Панель кнопок
            var buttonPanel = new Panel { Dock = DockStyle.Bottom, Height = 40 };
            btnAddHero = new Button { Text = "Добавить героя", Location = new Point(10, 7), Width = 100 };
            btnDeleteHero = new Button { Text = "Удалить героя", Location = new Point(120, 7), Width = 100 };
            btnHitHero = new Button { Text = "Нанести урон", Location = new Point(230, 7), Width = 100 };
            btnStatistics = new Button { Text = "Статистика", Location = new Point(340, 7), Width = 80 };

            btnAddHero.Click += (s, e) => AddHero?.Invoke(this, EventArgs.Empty);
            btnDeleteHero.Click += (s, e) => DeleteHero?.Invoke(this, EventArgs.Empty);
            btnHitHero.Click += (s, e) =>
            {
                if (SelectedHeroId > 0)
                {
                    HeroDamaged?.Invoke(this, 10); // Пример урона
                }
                else
                {
                    MessageBox.Show("Выберите героя для нанесения урона", "Предупреждение");
                }
            };
            btnStatistics.Click += (s, e) => ShowStatistics?.Invoke(this, EventArgs.Empty);

            buttonPanel.Controls.AddRange(new Control[] { btnAddHero, btnDeleteHero, btnHitHero, btnStatistics });

            // Панель пагинации
            paginationPanel = new Panel { Dock = DockStyle.Bottom, Height = 40 };

            var lblPageSize = new Label { Text = "На странице:", Location = new Point(10, 10), AutoSize = true };
            cmbPageSize = new ComboBox
            {
                Location = new Point(90, 7),
                Width = 60,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbPageSize.Items.AddRange(new object[] { 5, 10, 20, 50 });
            cmbPageSize.SelectedItem = PageSize;
            cmbPageSize.SelectedIndexChanged += (s, e) =>
            {
                PageSize = (int)cmbPageSize.SelectedItem;
                PageSizeChanged?.Invoke(this, PageSize);
            };

            btnFirst = new Button { Text = "«", Location = new Point(160, 7), Width = 30 };
            btnPrev = new Button { Text = "‹", Location = new Point(195, 7), Width = 30 };
            lblCurrentPage = new Label { Text = "1", Location = new Point(230, 10), AutoSize = true };
            lblTotalPages = new Label { Text = "/ 1", Location = new Point(245, 10), AutoSize = true };
            btnNext = new Button { Text = "›", Location = new Point(270, 7), Width = 30 };
            btnLast = new Button { Text = "»", Location = new Point(305, 7), Width = 30 };

            btnFirst.Click += (s, e) => PageChanged?.Invoke(this, 1);
            btnPrev.Click += (s, e) => PageChanged?.Invoke(this, CurrentPage - 1);
            btnNext.Click += (s, e) => PageChanged?.Invoke(this, CurrentPage + 1);
            btnLast.Click += (s, e) => PageChanged?.Invoke(this, int.MaxValue); // Будет обработано в презентере

            // StatusStrip
            statusStrip = new StatusStrip();
            statusStrip.Items.Add(new ToolStripStatusLabel());

            paginationPanel.Controls.AddRange(new Control[]
            {
                lblPageSize, cmbPageSize,
                btnFirst, btnPrev, lblCurrentPage, lblTotalPages, btnNext, btnLast
            });

            // MenuStrip
            menuStrip = new MenuStrip();
            var fileMenu = new ToolStripMenuItem("Файл");
            var heroesMenu = new ToolStripMenuItem("Герои");
            var viewMenu = new ToolStripMenuItem("Вид");

            heroesMenu.DropDownItems.Add("Добавить героя", null, (s, e) => AddHero?.Invoke(this, EventArgs.Empty));
            heroesMenu.DropDownItems.Add("Удалить героя", null, (s, e) => DeleteHero?.Invoke(this, EventArgs.Empty));
            heroesMenu.DropDownItems.Add("Нанести урон", null, (s, e) => HeroDamaged?.Invoke(this, 10));
            heroesMenu.DropDownItems.Add("-");
            heroesMenu.DropDownItems.Add("Обновить", null, (s, e) => LoadHeroes?.Invoke(this, EventArgs.Empty));

            viewMenu.DropDownItems.Add("Показать расы", null, (s, e) => ShowAllSpecies?.Invoke(this, EventArgs.Empty));
            viewMenu.DropDownItems.Add("Статистика", null, (s, e) => ShowStatistics?.Invoke(this, EventArgs.Empty));

            fileMenu.DropDownItems.Add("Выход", null, (s, e) => Application.Exit());

            menuStrip.Items.Add(fileMenu);
            menuStrip.Items.Add(heroesMenu);
            menuStrip.Items.Add(viewMenu);

            // Добавление контролов на форму
            this.Controls.AddRange(new Control[] {
                dataGridView, searchPanel, buttonPanel, paginationPanel, statusStrip, menuStrip
            });
            this.MainMenuStrip = menuStrip;

            // Инициализация колонок DataGridView
            InitializeDataGridViewColumns();

            // Загрузка данных
            LoadHeroes?.Invoke(this, EventArgs.Empty);
        }

        private void InitializeDataGridViewColumns()
        {
            dataGridView.Columns.Clear();
            dataGridView.AutoGenerateColumns = false;

            dataGridView.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Id",
                HeaderText = "ID",
                Width = 40
            });
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Name",
                HeaderText = "Имя героя",
                Width = 120
            });
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "SpeciesName",
                HeaderText = "Раса",
                Width = 100
            });
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Hp",
                HeaderText = "HP",
                Width = 60
            });
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Strange",
                HeaderText = "Сила",
                Width = 50
            });
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Genre",
                HeaderText = "Гендер",
                Width = 70
            });
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "TypeOfDamage",
                HeaderText = "Тип урона",
                Width = 120
            });
        }

        // Реализация методов интерфейса IMainView
        public void DisplayHeroes(List<HeroDto> heroes)
        {
            dataGridView.DataSource = heroes;
        }

        public void DisplayStatistics(string statistics)
        {
            var statsForm = new Form
            {
                Text = "Статистика героев",
                Size = new Size(600, 400),
                StartPosition = FormStartPosition.CenterParent,
                MaximizeBox = false,
                FormBorderStyle = FormBorderStyle.FixedDialog
            };

            var textBox = new TextBox
            {
                Multiline = true,
                Dock = DockStyle.Fill,
                Text = statistics,
                ReadOnly = true,
                ScrollBars = ScrollBars.Vertical,
                Font = new Font("Consolas", 9),
                BackColor = Color.White
            };

            statsForm.Controls.Add(textBox);
            statsForm.ShowDialog();
        }

        public void ShowMessage(string message, string title)
        {
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void UpdatePagination(int currentPage, int totalPages, int totalCount)
        {
            CurrentPage = currentPage;
            lblCurrentPage.Text = currentPage.ToString();
            lblTotalPages.Text = $"/ {totalPages}";

            btnFirst.Enabled = currentPage > 1;
            btnPrev.Enabled = currentPage > 1;
            btnNext.Enabled = currentPage < totalPages;
            btnLast.Enabled = currentPage < totalPages;

            // Обновление статусной строки
            if (statusStrip.Items.Count > 0)
            {
                (statusStrip.Items[0] as ToolStripStatusLabel).Text =
                    $"Всего героев: {totalCount} | Страница: {currentPage} из {totalPages}";
            }
        }

        public void ShowSpecies(List<SpeciesDto> species)
        {
            var speciesForm = new Form
            {
                Text = "Все расы",
                Size = new Size(400, 300),
                StartPosition = FormStartPosition.CenterParent
            };

            var grid = new DataGridView
            {
                Dock = DockStyle.Fill,
                DataSource = species,
                ReadOnly = true
            };

            speciesForm.Controls.Add(grid);
            speciesForm.ShowDialog();
        }
    }
}