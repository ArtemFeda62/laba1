using Shared.Interfases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
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
