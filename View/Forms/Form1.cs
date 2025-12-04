using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using View;
using ЛАБА1;


namespace View.Forms
{
    public partial class Form1 : Form, IHeroListView, ISpeciesView
    {
        private HeroPresenter _heroPresenter;
        private SpeciesPresenter _speciesPresenter;

        // Реализация событий IHeroListView
        public event EventHandler LoadHeroesRequested;
        public event EventHandler<int> HeroSelected;
        public event EventHandler AddHeroRequested;
        public event EventHandler<int> DeleteHeroRequested;
        public event EventHandler<(int heroId, double damage)> DamageHeroRequested;
        public event EventHandler ShowStatisticsRequested;
        public event EventHandler<int> PageChanged;
        public event EventHandler<int> PageSizeChanged;
        public event EventHandler<string> FindHeroesRequested;
        public event EventHandler GroupBySpeciesRequested;
        public event EventHandler GroupByDamageTypeRequested;
        public event EventHandler ShowWoundedHeroesRequested;
        public event EventHandler ShowStrongestHeroesRequested;
        public event EventHandler ShowAllSpeciesRequested;

        // Реализация событий ISpeciesView
        public event EventHandler LoadSpeciesRequested;
        public event EventHandler AddSpeciesRequested;
        public event EventHandler<int> DeleteSpeciesRequested;
        public event EventHandler<(int id, string name, string description)> UpdateSpeciesRequested;

        public Form1()
        {
            InitializeComponent();
            InitializePresenters();
            InitializeEvents();

            // Загружаем данные при старте
            LoadHeroesRequested?.Invoke(this, EventArgs.Empty);
        }

        private void InitializePresenters()
        {
            var kernel = new StandardKernel(new AppModule());
            var logicService = kernel.Get<IHeroLogicService>();
            var heroService = kernel.Get<IHeroService>();

            _heroPresenter = new HeroPresenter(this, logicService, heroService);
            _speciesPresenter = new SpeciesPresenter(this, logicService);
        }

        private void InitializeEvents()
        {
            // Связываем кнопки с событиями
            btnAddHero.Click += (s, e) => AddHeroRequested?.Invoke(this, EventArgs.Empty);
            btnDeleteHero.Click += (s, e) =>
            {
                if (dataGridView1.CurrentRow?.DataBoundItem is HeroDisplayItem hero)
                {
                    DeleteHeroRequested?.Invoke(this, hero.Id);
                }
            };
            btnDamageHero.Click += (s, e) =>
            {
                if (dataGridView1.CurrentRow?.DataBoundItem is HeroDisplayItem hero)
                {
                    // Используем нашу форму для ввода урона
                    using (var damageForm = new DamageHeroForm(hero.Name, hero.Hp))
                    {
                        if (damageForm.ShowDialog() == DialogResult.OK)
                        {
                            DamageHeroRequested?.Invoke(this, (hero.Id, damageForm.DamageAmount));
                        }
                    }
                }
            };
            btnStatistics.Click += (s, e) => ShowStatisticsRequested?.Invoke(this, EventArgs.Empty);
            btnFindHero.Click += (s, e) =>
            {
                var searchText = txtSearch.Text;
                FindHeroesRequested?.Invoke(this, searchText);
            };
            btnShowSpecies.Click += (s, e) => ShowAllSpeciesRequested?.Invoke(this, EventArgs.Empty);
            btnAddSpecies.Click += (s, e) => AddSpeciesRequested?.Invoke(this, EventArgs.Empty);

            // Для остальных кнопок аналогично...
        }

        // Реализация методов IHeroListView
        public void DisplayHeroes(List<HeroDisplayItem> heroes)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => dataGridView1.DataSource = heroes));
            }
            else
            {
                dataGridView1.DataSource = heroes;
            }
        }

        public void DisplayStatistics(HeroStatistics statistics)
        {
            using (var statsForm = new StatisticsForm(statistics))
            {
                statsForm.ShowDialog();
            }
        }

        public void ShowMessage(string message, string title)
        {
            MessageBox.Show(message, title,
                title.Contains("Ошибка") ? MessageBoxButtons.OK : MessageBoxButtons.OK,
                title.Contains("Ошибка") ? MessageBoxIcon.Error : MessageBoxIcon.Information);
        }

        // Реализация методов ISpeciesView
        public void DisplaySpecies(List<Species> species)
        {
            // Можно отобразить в отдельном окне или списке
            using (var speciesForm = new SpeciesListForm(species))
            {
                speciesForm.ShowDialog();
            }
        }

        // Реализация методов IHeroListView
        public void DisplayHeroes(List<HeroDisplayItem> heroes)
        {
            if (dataGridView1.InvokeRequired)
            {
                dataGridView1.Invoke(new Action(() =>
                {
                    dataGridView1.DataSource = heroes;
                }));
            }
            else
            {
                dataGridView1.DataSource = heroes;
            }
        }

        public void DisplayStatistics(HeroStatistics statistics)
        {
            // Открываем форму статистики
            var statsForm = new StatisticsForm(statistics);
            statsForm.ShowDialog();
        }

        public void ShowMessage(string message, string title)
        {
            MessageBox.Show(message, title,
                title == "Ошибка" ? MessageBoxIcon.Error : MessageBoxIcon.Information);
        }

        public void UpdatePagination(int currentPage, int totalPages, int totalHeroes)
        {
            lblPageInfo.Text = $"Страница {currentPage} из {totalPages} ({totalHeroes} героев)";
            btnPrev.Enabled = currentPage > 1;
            btnNext.Enabled = currentPage < totalPages;
        }

        // Обработчики событий формы
        private void btnAddHero_Click(object sender, EventArgs e)
        {
            AddHeroRequested?.Invoke(this, EventArgs.Empty);
        }

        private void btnDeleteHero_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow?.DataBoundItem is HeroDisplayItem hero)
            {
                DeleteHeroRequested?.Invoke(this, hero.Id);
            }
        }

        private void btnStatistics_Click(object sender, EventArgs e)
        {
            ShowStatisticsRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}