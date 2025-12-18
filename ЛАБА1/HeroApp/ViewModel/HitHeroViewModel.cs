using System.Windows.Input;
using HeroApp.WPF.DTO;
using HeroApp.WPF.View;

namespace HeroApp.WPF.ViewModel
{
    public class HitHeroViewModel : ViewModelBase
    {
        private HeroDto _hero;
        private double _damage;
        private double _remainingHp;

        public HeroDto Hero
        {
            get => _hero;
            private set => SetField(ref _hero, value);
        }

        public double Damage
        {
            get => _damage;
            set
            {
                if (SetField(ref _damage, value))
                {
                    RemainingHp = Hero.Hp - Damage;
                    OnPropertyChanged(nameof(WillDie));
                    OnPropertyChanged(nameof(WillBeWounded));
                    OnPropertyChanged(nameof(RemainingHpColor));
                }
            }
        }

        public double RemainingHp
        {
            get => _remainingHp;
            set => SetField(ref _remainingHp, value);
        }

        public bool WillDie => Damage >= Hero.Hp;
        public bool WillBeWounded => Damage > 0 && Damage < Hero.Hp && RemainingHp < 50;
        public string RemainingHpColor
        {
            get
            {
                if (RemainingHp <= 0) return "Red";
                if (RemainingHp < 20) return "Red";
                if (RemainingHp < 50) return "Orange";
                if (RemainingHp < 100) return "Green";
                return "DarkGreen";
            }
        }

        public ICommand ApplyDamageCommand { get; }
        public ICommand CancelCommand { get; }

        public HitHeroViewModel(HeroDto hero)
        {
            Hero = hero;
            Damage = 10;
            RemainingHp = Hero.Hp - Damage;

            ApplyDamageCommand = new RelayCommand(_ => ApplyDamage());
            CancelCommand = new RelayCommand(_ => Cancel());
        }

        private void ApplyDamage()
        {
            if (Damage <= 0)
            {
                StatusMessage = "Урон должен быть больше 0";
                return;
            }

            if (Damage > Hero.Hp * 2)
            {
                StatusMessage = "Урон не может превышать двойное значение HP";
                return;
            }

            ViewManager.CloseDialog(this, true);
        }

        private void Cancel()
        {
            ViewManager.CloseDialog(this, false);
        }
    }
}