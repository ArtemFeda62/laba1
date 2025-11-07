using System;
using System.Collections.Generic;
using System.Linq;
using DataAccessLayer;
using DataAccessLayer.EntityFramework;
using ЛАБА1;
using DataAccessLayer.Dapper;


namespace ЛАБА1
{
    public class Logic
    {
        private IRepository<Hero> _repository;
        private HeroContext _context;

        public Logic()
        {
            _repository = new EntityRepository<Hero>();
            _context = new HeroContext();
        }

        /// <summary>
        /// Получение героев с пагинацией
        /// </summary>
        /// <param name="pageNumber">Номер страницы</param>
        /// <param name="pageSize">Размер страницы</param>
        /// <returns>Список героев для указанной страницы</returns>
        public List<Hero> GetHeroesWithPagination(int pageNumber, int pageSize)
        {
            return _repository.ReadAll()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        /// <summary>
        /// Получение общего количества героев
        /// </summary>
        public int GetTotalHeroesCount()
        {
            return _repository.ReadAll().Count();
        }

        public void CreateHero(string name, int speciesId, string genre, int strange, string typeofdamage, double hp)
        {
            var hero = new Hero(name, speciesId, genre, strange, typeofdamage, hp);
            _repository.Add(hero);
        }

        public List<Species> GetAllSpecies()
        {
            return _context.Species.OrderBy(s => s.Name).ToList();
        }

        public void AddSpecies(string name, string description)
        {
            var species = new Species { Name = name, Description = description };
            _context.Species.Add(species);
            _context.SaveChanges();
        }

        public Species GetSpeciesById(int id)
        {
            return _context.Species.FirstOrDefault(s => s.Id == id);
        }

        public Species GetSpeciesByName(string name)
        {
            return _context.Species.FirstOrDefault(s => s.Name == name);
        }

        public List<string> GetAvailableSpeciesNames()
        {
            return _context.Species.Select(s => s.Name).ToList();
        }

        public Dictionary<string, List<Hero>> GroupHeroesBySpecies()
        {
            var heroes = _repository.ReadAll().ToList();
            return heroes
                .Where(h => h.Species != null)
                .GroupBy(h => h.Species.Name)
                .ToDictionary(g => g.Key, g => g.ToList());
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