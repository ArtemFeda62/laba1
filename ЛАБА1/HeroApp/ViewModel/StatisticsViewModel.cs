using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using HeroApp.WPF.DTO;
using HeroApp.WPF.View;
using Ninject;
using Shared.Interfases;

namespace HeroApp.WPF.ViewModel
{
    public class StatisticsViewModel : ViewModelBase
    {
        private readonly IHeroService _heroService;
        private HeroStatistics _statistics;

        public HeroStatistics Statistics
        {
            get => _statistics;
            set => SetField(ref _statistics, value);
        }

        public ObservableCollection<SpeciesStatDto> SpeciesStats { get; } = new();
        public ObservableCollection<DamageTypeStatDto> DamageTypeStats { get; } = new();
        public ObservableCollection<GenderStatDto> GenderStats { get; } = new();
        public ObservableCollection<HeroDto> LowHpHeroes { get; } = new();

        public ICommand LoadCommand { get; }
        public ICommand CloseCommand { get; }

        public StatisticsViewModel()
        {
            _heroService = Kernel.Get<IHeroService>();

            LoadCommand = new RelayCommand(async _ => await LoadStatisticsAsync());
            CloseCommand = new RelayCommand(_ => Close());

            LoadStatisticsAsync();
        }

        private async Task LoadStatisticsAsync()
        {
            await ExecuteAsync(() =>
            {
                Statistics = _heroService.GetStatistics();
                SpeciesStats.Clear();
                foreach (var stat in Statistics.SpeciesStats)
                {
                    SpeciesStats.Add(new SpeciesStatDto
                    {
                        Species = stat.Species,
                        Count = stat.Count,
                        AvgStrength = stat.AvgStrength,
                        AvgHp = stat.AvgHp
                    });
                }

                DamageTypeStats.Clear();
                foreach (var stat in Statistics.DamageTypeStats)
                {
                    DamageTypeStats.Add(new DamageTypeStatDto
                    {
                        DamageType = stat.DamageType,
                        Count = stat.Count,
                        TotalStrength = stat.TotalStrength
                    });
                }

                GenderStats.Clear();
                foreach (var stat in Statistics.GenderStats)
                {
                    GenderStats.Add(new GenderStatDto
                    {
                        Gender = stat.Gender,
                        Count = stat.Count,
                        Percentage = stat.Percentage
                    });
                }

                LowHpHeroes.Clear();
                foreach (var hero in Statistics.LowHpHeroes)
                {
                    LowHpHeroes.Add(new HeroDto
                    {
                        Id = hero.Id,
                        Name = hero.Name,
                        SpeciesName = hero.Species?.Name,
                        Hp = hero.Hp,
                        Strange = hero.Strange
                    });
                }

                StatusMessage = $"Статистика загружена: {Statistics.TotalHeroes} героев";

                return Task.CompletedTask;
            });
        }

        private void Close()
        {
            
        }
    }

    public class SpeciesStatDto
    {
        public string Species { get; set; }
        public int Count { get; set; }
        public double AvgStrength { get; set; }
        public double AvgHp { get; set; }
    }

    public class DamageTypeStatDto
    {
        public string DamageType { get; set; }
        public int Count { get; set; }
        public int TotalStrength { get; set; }
    }

    public class GenderStatDto
    {
        public string Gender { get; set; }
        public int Count { get; set; }
        public double Percentage { get; set; }
    }
}