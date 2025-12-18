using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using HeroApp.WPF.DTO;
using HeroApp.WPF.View;
using Shared.Domain;


namespace HeroApp.WPF.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private ObservableCollection<HeroDto> _heroes;
        private ObservableCollection<SpeciesDto> _species;
        private HeroDto _selectedHero;
        private string _searchText;
        private int _currentPage = 1;
        private int _pageSize = 10;
        private int _totalItems;
        private int _totalPages;

        public ObservableCollection<HeroDto> Heroes
        {
            get => _heroes;
            set => SetField(ref _heroes, value);
        }

        public ObservableCollection<SpeciesDto> Species
        {
            get => _species;
            set => SetField(ref _species, value);
        }

        public HeroDto SelectedHero
        {
            get => _selectedHero;
            set => SetField(ref _selectedHero, value);
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                if (SetField(ref _searchText, value))
                {
                    SearchHeroes();
                }
            }
        }

        public int CurrentPage
        {
            get => _currentPage;
            set => SetField(ref _currentPage, value);
        }

        public int PageSize
        {
            get => _pageSize;
            set
            {
                if (SetField(ref _pageSize, value))
                {
                    CurrentPage = 1;
                    LoadHeroes();
                }
            }
        }

        public int TotalItems
        {
            get => _totalItems;
            set => SetField(ref _totalItems, value);
        }

        public int TotalPages
        {
            get => _totalPages;
            set => SetField(ref _totalPages, value);
        }

        public ICommand LoadDataCommand { get; }
        public ICommand AddHeroCommand { get; }
        public ICommand EditHeroCommand { get; }
        public ICommand DeleteHeroCommand { get; }
        public ICommand HitHeroCommand { get; }
        public ICommand ShowStatisticsCommand { get; }
        public ICommand NextPageCommand { get; }
        public ICommand PreviousPageCommand { get; }
        public ICommand RefreshCommand { get; }
        public ICommand FirstPageCommand { get; }
        public ICommand LastPageCommand { get; }
        public ICommand ShowSpeciesCommand { get; }

        public MainViewModel()
        {
            Heroes = new ObservableCollection<HeroDto>();
            Species = new ObservableCollection<SpeciesDto>();

            LoadDataCommand = new RelayCommand(async _ => await LoadDataAsync());
            AddHeroCommand = new RelayCommand(_ => ShowAddHeroDialog());
            EditHeroCommand = new RelayCommand(_ => ShowEditHeroDialog(), _ => SelectedHero != null);
            DeleteHeroCommand = new RelayCommand(async _ => await DeleteHeroAsync(), _ => SelectedHero != null);
            HitHeroCommand = new RelayCommand(_ => ShowHitHeroDialog(), _ => SelectedHero != null);
            ShowStatisticsCommand = new RelayCommand(_ => ShowStatistics());
            ShowSpeciesCommand = new RelayCommand(_ => ShowSpecies());
            NextPageCommand = new RelayCommand(_ => NextPage());
            PreviousPageCommand = new RelayCommand(_ => PreviousPage());
            FirstPageCommand = new RelayCommand(_ => FirstPage());
            LastPageCommand = new RelayCommand(_ => LastPage());
            RefreshCommand = new RelayCommand(_ => LoadDataAsync());

            LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            await ExecuteAsync(async () =>
            {
                // Загружаем героев
                var heroesResult = Logic.GetHeroesWithPagination(CurrentPage, PageSize);
                var totalCount = Logic.GetTotalHeroesCount();

                TotalItems = totalCount;
                TotalPages = (int)Math.Ceiling((double)totalCount / PageSize);

                Heroes.Clear();
                foreach (var hero in heroesResult)
                {
                    Heroes.Add(ToHeroDto(hero));
                }

                // Загружаем расы
                var speciesList = Logic.GetAllSpecies();
                Species.Clear();
                foreach (var species in speciesList)
                {
                    Species.Add(ToSpeciesDto(species));
                }

                StatusMessage = $"Загружено {heroesResult.Count} героев, {speciesList.Count} рас";
            });
        }

        private async Task LoadHeroes()
        {
            await ExecuteAsync(() =>
            {
                var heroes = Logic.GetHeroesWithPagination(CurrentPage, PageSize);

                Heroes.Clear();
                foreach (var hero in heroes)
                {
                    Heroes.Add(ToHeroDto(hero));
                }

                return Task.CompletedTask;
            });
        }

        private void FirstPage()
        {
            CurrentPage = 1;
            LoadHeroes();
        }

        private void LastPage()
        {
            CurrentPage = TotalPages;
            LoadHeroes();
        }

        private void ShowSpecies()
        {
            var speciesVm = new SpeciesViewModel();
            ViewManager.ShowDialog<SpeciesViewModel>(speciesVm);

            // После закрытия диалога обновляем список рас в главном окне
            if (speciesVm.Species != null && speciesVm.Species.Any())
            {
                // Обновляем список рас в главной ViewModel
                Species.Clear();
                foreach (var species in speciesVm.Species)
                {
                    Species.Add(species);
                }

                StatusMessage = $"Обновлен список рас. Теперь доступно: {Species.Count} рас";
            }
        }

        private void SearchHeroes()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                LoadDataAsync();
                return;
            }

            ExecuteAsync(() =>
            {
                var foundHeroes = Logic.FindHeroesByName(SearchText);

                Heroes.Clear();
                foreach (var hero in foundHeroes)
                {
                    Heroes.Add(ToHeroDto(hero));
                }

                StatusMessage = $"Найдено {foundHeroes.Count} героев";

                return Task.CompletedTask;
            });
        }

        private void ShowAddHeroDialog()
        {
            var addHeroVm = new AddHeroViewModel(Species.ToList());
            var result = ViewManager.ShowDialog<AddHeroViewModel>(addHeroVm);

            if (result == true)
            {
                // Создаем героя в бизнес-логике
                Logic.CreateHero(
                    addHeroVm.Name,
                    addHeroVm.SelectedSpeciesId,
                    addHeroVm.Genre,
                    addHeroVm.Strange,
                    addHeroVm.TypeOfDamage,
                    addHeroVm.Hp
                );

                LoadDataAsync();
                StatusMessage = $"Герой {addHeroVm.Name} добавлен";
            }
        }

        private void ShowEditHeroDialog()
        {
            if (SelectedHero == null) return;

            var editHeroVm = new EditHeroViewModel(SelectedHero, Species.ToList());
            var result = ViewManager.ShowDialog<EditHeroViewModel>(editHeroVm);

            if (result == true)
            {
                // Обновляем героя в бизнес-логике
                var hero = Logic.GetHero(SelectedHero.Id);
                if (hero != null)
                {
                    hero.Name = editHeroVm.Name;
                    hero.SpeciesId = editHeroVm.SelectedSpeciesId;
                    hero.Genre = editHeroVm.Genre;
                    hero.Strange = editHeroVm.Strange;
                    hero.TypeOfDamage = editHeroVm.TypeOfDamage;
                    hero.Hp = editHeroVm.Hp;

                    Logic.UpdateHero(hero);
                    LoadDataAsync();
                    StatusMessage = $"Герой {hero.Name} обновлен";
                }
            }
        }

        private async Task DeleteHeroAsync()
        {
            if (SelectedHero == null) return;

            // Здесь можно добавить диалог подтверждения
            await ExecuteAsync(() =>
            {
                Logic.KillHero(SelectedHero.Id);
                LoadDataAsync();
                StatusMessage = $"Герой {SelectedHero.Name} удален";

                return Task.CompletedTask;
            });
        }

        private void ShowHitHeroDialog()
        {
            if (SelectedHero == null) return;

            var hitHeroVm = new HitHeroViewModel(SelectedHero);
            var result = ViewManager.ShowDialog<HitHeroViewModel>(hitHeroVm);

            if (result == true)
            {
                Logic.HitHero(SelectedHero.Id, hitHeroVm.Damage);
                LoadDataAsync();
                StatusMessage = $"Герою {SelectedHero.Name} нанесен урон {hitHeroVm.Damage}";
            }
        }

        private void ShowStatistics()
        {
            var statisticsVm = new StatisticsViewModel();
            ViewManager.ShowDialog<StatisticsViewModel>(statisticsVm);
        }

        private void NextPage()
        {
            if (CurrentPage < TotalPages)
            {
                CurrentPage++;
                LoadHeroes();
            }
        }

        private void PreviousPage()
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
                LoadHeroes();
            }
        }

        // Методы для преобразования между Domain и DTO
        private HeroDto ToHeroDto(Hero hero)
        {
            return new HeroDto
            {
                Id = hero.Id,
                Name = hero.Name,
                SpeciesName = hero.Species?.Name,
                Genre = hero.Genre,
                Strange = hero.Strange,
                Hp = hero.Hp,
                TypeOfDamage = hero.TypeOfDamage
            };
        }

        private SpeciesDto ToSpeciesDto(Species species)
        {
            return new SpeciesDto
            {
                Id = species.Id,
                Name = species.Name,
                Description = species.Description
            };
        }
    }
}