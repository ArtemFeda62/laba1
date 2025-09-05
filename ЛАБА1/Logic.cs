using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ЛАБА1
{
    internal class Logic
    {
        private HerosLibrary library;
        public Logic()
        {
            library = new HerosLibrary();
        }
        public void CreateHero(string name, string genre, string species, double hp, string typeofdamage, int strange)
        {
            var Hero = new Hero(name, species, genre, strange, typeofdamage);
            library.AddHero(Hero);
        }
        public Hero GetHero(int id) => library.GetHero(id);
        public List<Hero> GetListHeros() => library.GetListHeros();
        public void UpdateHero(Hero hero) => library.UpdateHero(hero);
        public void KillHero(int id) => library.KillHero(id);
        public void HitHero(int id, double damage) => library.HitHero(id, damage);
        // Бизнес-функция 1: Группировка героев по расам
        public Dictionary<string, List<Hero>> GroupHeroesBySpecies()
        {
            return library.GetListHeros()
                .GroupBy(h => h.Species)
                .ToDictionary(g => g.Key, g => g.ToList());
        }

        // Бизнес-функция 2: Группировка героев по типу урона
        public Dictionary<string, List<Hero>> GroupHeroesByDamageType()
        {
            return library.GetListHeros()
                .GroupBy(h => h.TypeOfDamage)
                .ToDictionary(g => g.Key, g => g.ToList());
        }

        // Бизнес-функция 3: Поиск героев по имени
        public List<Hero> FindHeroesByName(string name)
        {
            return library.GetListHeros()
                .Where(h => h.Name.ToLower().Contains(name.ToLower()))
                .ToList();
        }

        // Бизнес-функция 4: Герои с HP меньше указанного
        public List<Hero> GetHeroesWithLowHp(double maxHp)
        {
            return library.GetListHeros()
                .Where(h => h.Hp <= maxHp)
                .ToList();
        }

        // Бизнес-функция 5: Самые сильные герои
        public List<Hero> GetStrongestHeroes(int count)
        {
            return library.GetListHeros()
                .OrderByDescending(h => h.Strange)
                .Take(count)
                .ToList();
        }
    }

}
