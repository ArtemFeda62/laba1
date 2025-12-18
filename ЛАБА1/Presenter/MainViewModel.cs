// Presenter/MainViewModel.cs
using BusinessLogicLayer;
using Ninject;
using Shared;
using Shared.Domain;
using Shared.DTO;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using View.WPF;

namespace Presenter
{
    public class MainViewModel : BaseViewModel
    {
        private readonly Logic _logic;
        private ObservableCollection<HeroDTO> _heroes;
        private ObservableCollection<SpeciesDTO> _species;
        private HeroDTO _selectedHero;
        private SpeciesDTO _selectedSpecies;
        private string _searchText;
        private int _currentPage = 1;
        private int _pageSize = 10;
        private int _totalCount;

        public MainViewModel()
        {
            IKernel ninjectKernel = new StandardKernel(new SimpleConfigModule());
            _logic = ninjectKernel.Get<Logic>();

            Heroes = new ObservableCollection<HeroDTO>();
            Species = new ObservableCollection<SpeciesDTO>();

            InitializeCommands();
            LoadData();
        }

        #region Свойства

        public ObservableCollection<HeroDTO> Heroes
        {
            get => _heroes;
            set => SetProperty(ref _heroes, value);
        }

        public ObservableCollection<SpeciesDTO> Species
        {
            get => _species;
            set => SetProperty(ref _species, value);
        }

        public HeroDTO SelectedHero
        {
            get => _selectedHero;
            set => SetProperty(ref _selectedHero, value);
        }

        public SpeciesDTO SelectedSpecies
        {
            get => _selectedSpecies;
            set => SetProperty(ref _selectedSpecies, value);
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                if (SetProperty(ref _searchText, value))
                {
                    // Автоматический поиск при изменении текста
                    SearchHeroesCommand.Execute(null);
                }
            }
        }

        public int CurrentPage
        {
            get => _currentPage;
            set => SetProperty(ref _currentPage, value);
        }

        public int PageSize
        {
            get => _pageSize;
            set
            {
                if (SetProperty(ref _pageSize, value))
                {
                    LoadHeroes();
                }
            }
        }

        public int TotalCount
        {
            get => _totalCount;
            set => SetProperty(ref _totalCount, value);
        }

        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);

        #endregion

        #region Команды

        public ICommand AddHeroCommand { get; private set; }
        public ICommand EditHeroCommand { get; private set; }
        public ICommand DeleteHeroCommand { get; private set; }
        public ICommand HitHeroCommand { get; private set; }
        public ICommand SearchHeroesCommand { get; private set; }
        public ICommand RefreshCommand { get; private set; }
        public ICommand AddSpeciesCommand { get; private set; }
        public ICommand EditSpeciesCommand { get; private set; }
        public ICommand DeleteSpeciesCommand { get; private set; }
        public ICommand NextPageCommand { get; private set; }
        public ICommand PrevPageCommand { get; private set; }
        public ICommand ShowStatisticsCommand { get; private set; }

        private void InitializeCommands()
        {
            AddHeroCommand = new RelayCommand(AddHero);
            EditHeroCommand = new RelayCommand(EditHero, CanEditHero);
            DeleteHeroCommand = new RelayCommand(DeleteHero, CanDeleteHero);
            HitHeroCommand = new RelayCommand(HitHero, CanHitHero);
            SearchHeroesCommand = new RelayCommand(SearchHeroes);
            RefreshCommand = new RelayCommand(Refresh);
            AddSpeciesCommand = new RelayCommand(AddSpecies);
            EditSpeciesCommand = new RelayCommand(EditSpecies);
            DeleteSpeciesCommand = new RelayCommand(DeleteSpecies, CanDeleteSpecies);
            NextPageCommand = new RelayCommand(NextPage, CanGoNext);
            PrevPageCommand = new RelayCommand(PrevPage, CanGoPrev);
            ShowStatisticsCommand = new RelayCommand(ShowStatistics);
        }

        #endregion

        #region Методы загрузки данных

        private void LoadData()
        {
            LoadHeroes();
            LoadSpecies();
        }

        private void LoadHeroes()
        {
            var result = _logic.GetHeroesWithPagination(CurrentPage, PageSize);
            var heroesList = _logic.GetListHeros().Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();

            Heroes.Clear();

            foreach (var hero in heroesList)
            {
                var heroDto = ConvertToDTO(hero);
                Heroes.Add(heroDto);
            }

            TotalCount = _logic.GetTotalHeroesCount();
        }

        private void LoadSpecies()
        {
            var speciesList = _logic.GetAllSpecies();
            Species.Clear();

            foreach (var species in speciesList)
            {
                var speciesDto = ConvertToDTO(species);
                Species.Add(speciesDto);
            }
        }

        #endregion

        #region Конвертация между DTO и Domain моделями

        private HeroDTO ConvertToDTO(Hero hero)
        {
            return new HeroDTO
            {
                Id = hero.Id,
                Name = hero.Name,
                SpeciesId = hero.SpeciesId,
                SpeciesName = hero.Species?.Name,
                Genre = hero.Genre,
                Strange = hero.Strange,
                Hp = hero.Hp,
                TypeOfDamage = hero.TypeOfDamage
            };
        }

        private SpeciesDTO ConvertToDTO(Shared.Domain.Species species)
        {
            return new SpeciesDTO
            {
                Id = species.Id,
                Name = species.Name,
                Description = species.Description
            };
        }

        private Hero ConvertToDomain(HeroDTO heroDto)
        {
            return new Hero(
                heroDto.Name,
                heroDto.SpeciesId,
                heroDto.Genre,
                heroDto.Strange,
                heroDto.TypeOfDamage,
                heroDto.Hp)
            {
                Id = heroDto.Id
            };
        }

        private Shared.Domain.Species ConvertToDomain(SpeciesDTO speciesDto)
        {
            return new Shared.Domain.Species
            {
                Id = speciesDto.Id,
                Name = speciesDto.Name,
                Description = speciesDto.Description
            };
        }

        #endregion

        #region Реализация команд

        private void AddHero()
        {
            var addHeroVm = new AddEditHeroViewModel(this, _logic);
            ViewManager.ShowDialog(addHeroVm);
            if (addHeroVm.IsSaved)
            {
                LoadHeroes();
            }
        }

        private bool CanEditHero()
        {
            return SelectedHero != null;
        }

        private void EditHero()
        {
            if (SelectedHero == null) return;

            var editHeroVm = new AddEditHeroViewModel(this, _logic, SelectedHero);
            ViewManager.ShowDialog(editHeroVm);
            if (editHeroVm.IsSaved)
            {
                LoadHeroes();
            }
        }

        private bool CanDeleteHero()
        {
            return SelectedHero != null;
        }

        private void DeleteHero()
        {
            if (SelectedHero == null) return;

            // TODO: Добавить диалог подтверждения
            _logic.KillHero(SelectedHero.Id);
            LoadHeroes();
        }

        private bool CanHitHero()
        {
            return SelectedHero != null && SelectedHero.Hp > 0;
        }

        private void HitHero()
        {
            if (SelectedHero == null) return;

            var hitHeroVm = new HitHeroViewModel(SelectedHero, _logic);
            ViewManager.ShowDialog(hitHeroVm);
            if (hitHeroVm.IsDamaged)
            {
                LoadHeroes();
            }
        }

        private void SearchHeroes()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                LoadHeroes();
                return;
            }

            var foundHeroes = _logic.FindHeroesByName(SearchText);
            Heroes.Clear();

            foreach (var hero in foundHeroes)
            {
                var heroDto = ConvertToDTO(hero);
                Heroes.Add(heroDto);
            }

            TotalCount = foundHeroes.Count;
        }

        private void Refresh()
        {
            SearchText = string.Empty;
            LoadData();
        }

        private void AddSpecies()
        {
            var addSpeciesVm = new AddEditSpeciesViewModel(_logic);
            ViewManager.ShowDialog(addSpeciesVm);
            if (addSpeciesVm.IsSaved)
            {
                LoadSpecies();
            }
        }

        private void EditSpecies()
        {
            if (SelectedSpecies == null) return;

            var editSpeciesVm = new AddEditSpeciesViewModel(_logic, SelectedSpecies);
            ViewManager.ShowDialog(editSpeciesVm);
            if (editSpeciesVm.IsSaved)
            {
                LoadSpecies();
            }
        }

        private bool CanDeleteSpecies()
        {
            return SelectedSpecies != null;
        }

        private void DeleteSpecies()
        {
            if (SelectedSpecies == null) return;

            // TODO: Добавить диалог подтверждения
            _logic.DeleteSpecies(SelectedSpecies.Id);
            LoadSpecies();
            LoadHeroes(); // Обновляем героев, так как могли измениться связи
        }

        private bool CanGoNext()
        {
            return CurrentPage < TotalPages;
        }

        private void NextPage()
        {
            if (CanGoNext())
            {
                CurrentPage++;
                LoadHeroes();
            }
        }

        private bool CanGoPrev()
        {
            return CurrentPage > 1;
        }

        private void PrevPage()
        {
            if (CanGoPrev())
            {
                CurrentPage--;
                LoadHeroes();
            }
        }

        private void ShowStatistics()
        {
            var statisticsVm = new StatisticsViewModel(_logic);
            ViewManager.ShowDialog(statisticsVm);
        }

        #endregion
    }
}