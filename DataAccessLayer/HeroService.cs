using DataAccessLayer;
using System.Collections.Generic;
using System.Linq;
using ЛАБА1;

namespace BusinessLogicLayer.Services
{
    public class HeroStatistics
    {
        public int TotalHeroes { get; set; }
        public double AverageStrength { get; set; }
        public double AverageHp { get; set; }
        public int MaxStrength { get; set; }
        public double MinHp { get; set; }
        public List<SpeciesStat> SpeciesStats { get; set; }
        public List<DamageTypeStat> DamageTypeStats { get; set; }
        public List<GenderStat> GenderStats { get; set; }
        public List<Hero> LowHpHeroes { get; set; }
    }

    public class SpeciesStat
    {
        public string Species { get; set; }
        public int Count { get; set; }
        public double AvgStrength { get; set; }
        public double AvgHp { get; set; }
    }

    public class DamageTypeStat
    {
        public string DamageType { get; set; }
        public int Count { get; set; }
        public int TotalStrength { get; set; }
    }

    public class GenderStat
    {
        public string Gender { get; set; }
        public int Count { get; set; }
        public double Percentage { get; set; }
    }

    public class HeroService : IHeroService
    {
        private readonly IHeroRepository _repository;

        public HeroService(IHeroRepository repository)
        {
            _repository = repository;
        }

        public HeroStatistics GetStatistics()
        {
            var heroes = _repository.ReadAll().ToList();

            return new HeroStatistics
            {
                TotalHeroes = heroes.Count,
                AverageStrength = heroes.Average(h => h.Strange),
                AverageHp = heroes.Average(h => h.Hp),
                MaxStrength = heroes.Max(h => h.Strange),
                MinHp = heroes.Min(h => h.Hp),
                SpeciesStats = heroes
                    .Where(h => h.Species != null)
                    .GroupBy(h => h.Species.Name)
                    .Select(g => new SpeciesStat
                    {
                        Species = g.Key,
                        Count = g.Count(),
                        AvgStrength = g.Average(h => h.Strange),
                        AvgHp = g.Average(h => h.Hp)
                    }).ToList(),
                DamageTypeStats = heroes
                    .GroupBy(h => h.TypeOfDamage)
                    .Select(g => new DamageTypeStat
                    {
                        DamageType = g.Key,
                        Count = g.Count(),
                        TotalStrength = g.Sum(h => h.Strange)
                    }).ToList(),
                GenderStats = heroes
                    .GroupBy(h => h.Genre)
                    .Select(g => new GenderStat
                    {
                        Gender = g.Key,
                        Count = g.Count(),
                        Percentage = (double)g.Count() / heroes.Count * 100
                    }).ToList(),
                LowHpHeroes = heroes.Where(h => h.Hp < 50).ToList()
            };
        }
    }
}