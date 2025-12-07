using DataAccessLayer.Dapper;
using DataAccessLayer.EntityFramework;
using Shared;
using Shared.Domain;
using Shared.Interfases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public class Logic
    {
        private IHeroRepository _heroRepository;
        private ISpeciesRepository _speciesRepository;

        public Logic(IHeroRepository heroRepository, ISpeciesRepository speciesRepository)
        {
            _heroRepository = heroRepository;
            _speciesRepository = speciesRepository;
        }

        public Logic()
        {
            _heroRepository = new DapperHeroRepository();
            _speciesRepository = new DapperSpeciesRepository();
        }

        // Метод для пагинации через Dapper
        public (List<Hero> heroes, int totalCount) GetHeroesWithDapperPagination(int pageNumber, int pageSize)
        {
            if (_heroRepository is DapperHeroRepository dapperRepo)
            {
                var result = dapperRepo.ReadAllWithPagination(pageNumber, pageSize);
                return (result.heroes.ToList(), result.totalCount);
            }
            else
            {
                // Для Entity Framework
                var allHeroes = _heroRepository.ReadAll().ToList();
                var pagedHeroes = allHeroes
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                return (pagedHeroes, allHeroes.Count);
            }
        }

        public List<Hero> GetHeroesWithPagination(int pageNumber, int pageSize)
        {
            return _heroRepository.ReadAll()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        public int GetTotalHeroesCount()
        {
            return _heroRepository.ReadAll().Count();
        }

        // Методы для работы с героями
        public void CreateHero(string name, int speciesId, string genre, int strange, string typeofdamage, double hp)
        {
            var hero = new Hero(name, speciesId, genre, strange, typeofdamage, hp);
            _heroRepository.Add(hero);
        }

        public Hero GetHero(int id) => _heroRepository.ReadById(id);

        public List<Hero> GetListHeros() => _heroRepository.ReadAll().ToList();

        public void UpdateHero(Hero hero) => _heroRepository.Update(hero);

        public void KillHero(int id) => _heroRepository.Delete(id);

        public void HitHero(int id, double damage)
        {
            var hero = _heroRepository.ReadById(id);
            if (hero != null)
            {
                hero.Hp -= damage;
                _heroRepository.Update(hero);
            }
        }

        // Методы для работы с расами
        public List<Species> GetAllSpecies()
        {
            return _speciesRepository.GetAllOrderedByName().ToList();
        }

        public void AddSpecies(string name, string description)
        {
            var species = new Species { Name = name, Description = description };
            _speciesRepository.Add(species);
        }

        public Species GetSpeciesById(int id)
        {
            return _speciesRepository.ReadById(id);
        }

        public Species GetSpeciesByName(string name)
        {
            return _speciesRepository.ReadAll().FirstOrDefault(s => s.Name == name);
        }

        public void UpdateSpecies(Species species)
        {
            _speciesRepository.Update(species);
        }

        public void DeleteSpecies(int id)
        {
            _speciesRepository.Delete(id);
        }

        public List<string> GetAvailableSpeciesNames()
        {
            return _speciesRepository.GetAllOrderedByName().Select(s => s.Name).ToList();
        }

        // Методы группировки и поиска
        public Dictionary<string, List<Hero>> GroupHeroesBySpecies()
        {
            var heroes = _heroRepository.ReadAll().ToList();
            return heroes
                .Where(h => h.Species != null)
                .GroupBy(h => h.Species.Name)
                .ToDictionary(g => g.Key, g => g.ToList());
        }

        public Dictionary<string, List<Hero>> GroupHeroesByDamageType()
        {
            return _heroRepository.ReadAll()
                .GroupBy(h => h.TypeOfDamage)
                .ToDictionary(g => g.Key, g => g.ToList());
        }

        public List<Hero> FindHeroesByName(string name)
        {
            return _heroRepository.ReadAll()
                .Where(h => h.Name.ToLower().Contains(name.ToLower()))
                .ToList();
        }

        public List<Hero> GetHeroesWithLowHp(double maxHp)
        {
            return _heroRepository.ReadAll()
                .Where(h => h.Hp <= maxHp)
                .ToList();
        }

        public List<Hero> GetStrongestHeroes(int count)
        {
            return _heroRepository.ReadAll()
                .OrderByDescending(h => h.Strange)
                .Take(count)
                .ToList();
        }
    }
}
