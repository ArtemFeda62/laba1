using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HeroApp.WPF.DTO
{
    public class HeroDto : INotifyPropertyChanged
    {
        private int _id;
        private string _name;
        private string _speciesName;
        private string _genre;
        private int _strange;
        private double _hp;
        private string _typeOfDamage;

        public int Id
        {
            get => _id;
            set => SetField(ref _id, value);
        }

        public string Name
        {
            get => _name;
            set => SetField(ref _name, value);
        }

        public string SpeciesName
        {
            get => _speciesName;
            set => SetField(ref _speciesName, value);
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

        public double Hp
        {
            get => _hp;
            set => SetField(ref _hp, value);
        }

        public string TypeOfDamage
        {
            get => _typeOfDamage;
            set => SetField(ref _typeOfDamage, value);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}