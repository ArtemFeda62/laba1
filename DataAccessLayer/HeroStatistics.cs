using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ЛАБА1;

namespace DataAccessLayer.Service
{
    public class HeroStatistics
    {
        public int TotalHeroes { get; set; }
        public double AverageHp { get; set; }
        public double AverageStrength { get; set; }
        public Hero StrongestHero { get; set; }
        public int TotalAliveHeroes { get; set; }
        public int ArmedHeroesCount { get; set; } 
    }
}