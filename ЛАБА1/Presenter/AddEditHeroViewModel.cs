using BusinessLogicLayer;
using Shared;
using Shared.DTO;
using System.Collections.ObjectModel;
using System.Windows.Input;
using View.WPF;

namespace Presenter
{
    public class AddEditHeroViewModel : BaseViewModel
    {
        private readonly Logic _logic;
        private readonly MainViewModel _mainVm;
        private HeroDTO _hero;
        private bool _isEditMode;

        public AddEditHeroViewModel(MainViewModel mainVm, Logic logic)
        {
            _mainVm = mainVm;
            _logic = logic;
            _isEditMode = false;
            _hero = new HeroDTO();
            Initialize();
        }

        public AddEditHeroViewModel(MainViewModel mainVm, Logic logic, HeroDTO heroToEdit)
        {
            _mainVm = mainVm;
            _logic = logic;
            _isEditMode = true;
            _hero = heroToEdit;
            Initialize();
        }

        private void Initialize()
        {
            SaveCommand = new RelayCommand(Save, CanSave);
            CancelCommand = new RelayCommand(Cancel);
            LoadSpecies();
        }

        #region Свойства

        public HeroDTO Hero
        {
            get => _hero;
            set => SetProperty(ref _hero, value);
        }

        public ObservableCollection<SpeciesDTO> AvailableSpecies { get; private set; }

        public SpeciesDTO SelectedSpecies
        {
            get => AvailableSpecies?.FirstOrDefault(s => s.Id == Hero.SpeciesId);
            set
            {
                if (value != null)
                {
                    Hero.SpeciesId = value.Id;
                    Hero.SpeciesName = value.Name;
                    OnPropertyChanged(nameof(SelectedSpecies));
                }
            }
        }

        public bool IsEditMode => _isEditMode;
        public bool IsSaved { get; private set; }

        #endregion

        #region Команды

        public ICommand SaveCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }

        #endregion

        #region Методы

        private void LoadSpecies()
        {
            var speciesList = _logic.GetAllSpecies();
            AvailableSpecies = new ObservableCollection<SpeciesDTO>();

            foreach (var species in speciesList)
            {
                var speciesDto = new SpeciesDTO
                {
                    Id = species.Id,
                    Name = species.Name,
                    Description = species.Description
                };
                AvailableSpecies.Add(speciesDto);
            }

            OnPropertyChanged(nameof(AvailableSpecies));
            OnPropertyChanged(nameof(SelectedSpecies));
        }

        private bool CanSave()
        {
            return !string.IsNullOrWhiteSpace(Hero.Name) &&
                   Hero.SpeciesId > 0 &&
                   !string.IsNullOrWhiteSpace(Hero.Genre) &&
                   Hero.Strange > 0 &&
                   Hero.Hp > 0 &&
                   !string.IsNullOrWhiteSpace(Hero.TypeOfDamage);
        }

        private void Save()
        {
            if (!CanSave()) return;

            try
            {
                if (_isEditMode)
                {
                    var domainHero = new Hero(
                        Hero.Name,
                        Hero.SpeciesId,
                        Hero.Genre,
                        Hero.Strange,
                        Hero.TypeOfDamage,
                        Hero.Hp)
                    {
                        Id = Hero.Id
                    };
                    _logic.UpdateHero(domainHero);
                }
                else
                {
                    _logic.CreateHero(
                        Hero.Name,
                        Hero.SpeciesId,
                        Hero.Genre,
                        Hero.Strange,
                        Hero.TypeOfDamage,
                        Hero.Hp);
                }

                IsSaved = true;
                ViewManager.CloseWindow(this);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving hero: {ex.Message}");
            }
        }

        private void Cancel()
        {
            ViewManager.CloseWindow(this);
        }

        #endregion
    }
}