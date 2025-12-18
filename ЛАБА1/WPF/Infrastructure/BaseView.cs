using BusinessLogicLayer;
using Presenter.DTO;
using Presenter.Infrastructure;
using Presenter.Interfaces;
using Shared.Domain;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Presenter.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly Logic _logic;
        private readonly IViewManager _viewManager;
        private HeroDto _selectedHero;

        public ObservableCollection<HeroDto> Heroes { get; }

        public HeroDto SelectedHero
        {
            get => _selectedHero;
            set => Set(ref _selectedHero, value);
        }

        public ICommand RefreshCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand DeleteCommand { get; }

        public MainViewModel(Logic logic, IViewManager viewManager)
        {
            _logic = logic;
            _viewManager = viewManager;
            Heroes = new ObservableCollection<HeroDto>();

            RefreshCommand = new RelayCommand(obj => LoadData());
            SaveCommand = new RelayCommand(obj => SaveData(), obj => SelectedHero != null);
            DeleteCommand = new RelayCommand(obj => DeleteData(), obj => SelectedHero != null);

            LoadData();
        }

        private void LoadData()
        {
            Heroes.Clear();
            var heroesFromDb = _logic.GetListHeros();
            foreach (var h in heroesFromDb)
            {
                Heroes.Add(new HeroDto
                {
                    Id = h.Id,
                    Name = h.Name,
                    Strange = h.Strange,
                    Hp = h.Hp,
                    Genre = h.Genre,
                    TypeOfDamage = h.TypeOfDamage,
                    SpeciesId = h.SpeciesId,
                    SpeciesName = h.Species?.Name
                });
            }
        }

        private void SaveData()
        {
            var hero = new Hero
            {
                Id = SelectedHero.Id,
                Name = SelectedHero.Name,
                Strange = SelectedHero.Strange,
                Hp = SelectedHero.Hp,
                Genre = SelectedHero.Genre,
                TypeOfDamage = SelectedHero.TypeOfDamage,
                SpeciesId = SelectedHero.SpeciesId
            };
            _logic.UpdateHero(hero);
            LoadData();
        }

        private void DeleteData()
        {
            _logic.KillHero(SelectedHero.Id);
            Heroes.Remove(SelectedHero);
        }
    }
}