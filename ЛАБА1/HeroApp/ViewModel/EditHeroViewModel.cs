using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using HeroApp.WPF.DTO;

namespace HeroApp.WPF.ViewModel
{
    public class EditHeroViewModel : ViewModelBase
    {
        private HeroDto _hero;
        private string _name;
        private int _selectedSpeciesId;
        private string _genre;
        private int _strange;
        private string _typeOfDamage;
        private double _hp;
        private List<SpeciesDto> _availableSpecies;

        public HeroDto OriginalHero
        {
            get => _hero;
            private set => SetField(ref _hero, value);
        }

        public string Name
        {
            get => _name;
            set => SetField(ref _name, value);
        }

        public int SelectedSpeciesId
        {
            get => _selectedSpeciesId;
            set => SetField(ref _selectedSpeciesId, value);
        }

        public string Genre
        {
            get => _genre;
            set => SetField(ref _genre, value);
        }

        public int Strange
        {
            get => _strange;
            set => SetField(ref _strange, value);
        }

        public string TypeOfDamage
        {
            get => _typeOfDamage;
            set => SetField(ref _typeOfDamage, value);
        }

        public double Hp
        {
            get => _hp;
            set => SetField(ref _hp, value);
        }

        public List<SpeciesDto> AvailableSpecies
        {
            get => _availableSpecies;
            set => SetField(ref _availableSpecies, value);
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public EditHeroViewModel(HeroDto hero, List<SpeciesDto> availableSpecies)
        {
            OriginalHero = hero;
            AvailableSpecies = availableSpecies;

            // Копируем данные героя
            Name = hero.Name;
            SelectedSpeciesId = availableSpecies.FirstOrDefault(s => s.Name == hero.SpeciesName)?.Id ?? 0;
            Genre = hero.Genre;
            Strange = hero.Strange;
            TypeOfDamage = hero.TypeOfDamage;
            Hp = hero.Hp;

            SaveCommand = new RelayCommand(_ => Save());
            CancelCommand = new RelayCommand(_ => Cancel());
        }

        private void Save()
        {
            // Валидация
            if (string.IsNullOrWhiteSpace(Name) ||
                SelectedSpeciesId == 0 ||
                string.IsNullOrWhiteSpace(Genre) ||
                string.IsNullOrWhiteSpace(TypeOfDamage) ||
                Hp <= 0)
            {
                StatusMessage = "Заполните все поля корректно";
                return;
            }

            // Обновляем оригинальный объект
            OriginalHero.Name = Name;
            OriginalHero.Genre = Genre;
            OriginalHero.Strange = Strange;
            OriginalHero.TypeOfDamage = TypeOfDamage;
            OriginalHero.Hp = Hp;

            var selectedSpecies = AvailableSpecies.FirstOrDefault(s => s.Id == SelectedSpeciesId);
            OriginalHero.SpeciesName = selectedSpecies?.Name;

            ViewManager.CloseDialog(this, true);
        }

        private void Cancel()
        {
            ViewManager.CloseDialog(this, false);
        }
    }
}