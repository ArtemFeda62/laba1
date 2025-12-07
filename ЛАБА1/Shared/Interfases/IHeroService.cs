using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Shared.Interfases
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

    public interface IHeroService
    {
        HeroStatistics GetStatistics();
    }
}