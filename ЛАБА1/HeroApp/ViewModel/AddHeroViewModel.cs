using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using HeroApp.WPF.DTO;
using HeroApp.WPF.View;

namespace HeroApp.WPF.ViewModel
{
    public class AddHeroViewModel : ViewModelBase
    {
        private string _name;
        private int _selectedSpeciesId;
        private string _genre;
        private int _strange = 50;
        private string _typeOfDamage;
        private double _hp = 100;
        private List<SpeciesDto> _availableSpecies;

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

        public AddHeroViewModel(List<SpeciesDto> availableSpecies)
        {
            AvailableSpecies = availableSpecies;
            SelectedSpeciesId = AvailableSpecies.FirstOrDefault()?.Id ?? 0;

            SaveCommand = new RelayCommand(_ => Save());
            CancelCommand = new RelayCommand(_ => Cancel());
        }

        private void Save()
        {
            // Валидация
            if (string.IsNullOrWhiteSpace(Name) ||
                SelectedSpeciesId == 0 ||
                string.IsNullOrWhiteSpace(Genre) ||
                string.IsNullOrWhiteSpace(TypeOfDamage))
            {
                StatusMessage = "Заполните все обязательные поля";
                return;
            }

            // Закрываем диалог с результатом OK
            ViewManager.CloseDialog(this, true);
        }

        private void Cancel()
        {
            ViewManager.CloseDialog(this, false);
        }
    }
}