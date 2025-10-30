using System;
using System.Collections.Generic;
using System.Linq;
using DataAccessLayer;
using DataAccessLayer.EntityFramework;
using ЛАБА1;

namespace ЛАБА1
{
    public class Logic
    {
        private IRepository<Hero> _repository;
        public Logic()
        {
            _repository = new EntityRepository<Hero>();
        }
        public void CreateHero(string name, string genre, string species, double hp, string typeofdamage, int strange)
        {
            var hero = new Hero(name, species, genre, strange, typeofdamage, hp);
            _repository.Add(hero);
        }
        public Hero GetHero(int id) => _repository.ReadById(id);
        public List<Hero> GetListHeros() => _repository.ReadAll().ToList();
        public void UpdateHero(Hero hero) => _repository.Update(hero);
        public void KillHero(int id) => _repository.Delete(id);
        public void HitHero(int id, double damage)
        {
            var hero = _repository.ReadById(id);
            if (hero != null)
            {
                hero.Hp -= damage;
                _repository.Update(hero);
            }
        }
        public Dictionary<string, List<Hero>> GroupHeroesBySpecies()
        {
            return _repository.ReadAll()
                .GroupBy(h => h.Species)
                .ToDictionary(g => g.Key, g => g.ToList());
        }

        public Dictionary<string, List<Hero>> GroupHeroesByDamageType()
        {
            return _repository.ReadAll()
                .GroupBy(h => h.TypeOfDamage)
                .ToDictionary(g => g.Key, g => g.ToList());
        }
        public List<Hero> FindHeroesByName(string name)
        {
            return _repository.ReadAll()
                .Where(h => h.Name.ToLower().Contains(name.ToLower()))
                .ToList();
        }
        public List<Hero> GetHeroesWithLowHp(double maxHp)
        {
            return _repository.ReadAll()
                .Where(h => h.Hp <= maxHp)
                .ToList();
        }
        public List<Hero> GetStrongestHeroes(int count)
        {
            return _repository.ReadAll()
                .OrderByDescending(h => h.Strange)
                .Take(count)
                .ToList();
        }
    }
}