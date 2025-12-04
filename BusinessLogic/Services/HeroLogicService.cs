using DataAccess.Dapper;
using DataAccess.EntityFramework;
using Domain;
using Domain.Models;
using Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogic.Services
{
    public class HeroLogicService : IHeroLogicService
    {
        private IHeroRepository _heroRepository;
        private ISpeciesRepository _speciesRepository;
        private HeroContext _context;

        public HeroLogicService(IHeroRepository heroRepository, ISpeciesRepository speciesRepository)
        {
            _heroRepository = heroRepository;
            _speciesRepository = speciesRepository;
            _context = new HeroContext();
        }

        public HeroLogicService()
        {
            _heroRepository = new EntityHeroRepository();
            _speciesRepository = new EntitySpeciesRepository();
            _context = new HeroContext();
        }

        // Метод для пагинации через Dapper
        public (List<Hero> heroes, int totalCount) GetHeroesWithDapperPagination(int pageNumber, int pageSize)
        {
            if (_heroRepository is DapperHeroRepository dapperRepo)
            {
                var result = dapperRepo.GetAllWithPagination(pageNumber, pageSize);
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
            var hero = new Hero
            {
                Name = name,
                SpeciesId = speciesId,
                Genre = genre,
                Strange = strange,
                TypeOfDamage = typeofdamage,
                Hp = hp
            };
            _heroRepository.Add(hero);
        }

        public Hero GetHero(int id) => _heroRepository.ReadById(id);

        public List<Hero> GetAllHeroes() => _heroRepository.ReadAll().ToList();

        public void UpdateHero(Hero hero) => _heroRepository.Update(hero);

        public void DeleteHero(int id) => _heroRepository.Delete(id);

        public void ApplyDamage(int id, double damage)
        {
            var hero = _heroRepository.ReadById(id);
            if (hero != null)
            {
                hero.Hp -= damage;
                if (hero.Hp < 0) hero.Hp = 0;
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


        public HeroStatistics GetStatistics()
        {
            var heroes = _heroRepository.ReadAll().ToList();

            if (heroes.Count == 0)
            {
                return new HeroStatistics
                {
                    TotalHeroes = 0,
                    AverageStrength = 0,
                    AverageHp = 0,
                    MaxStrength = 0,
                    MinHp = 0,
                    SpeciesStats = new List<SpeciesStat>(),
                    DamageTypeStats = new List<DamageTypeStat>(),
                    GenderStats = new List<GenderStat>(),
                    LowHpHeroes = new List<Hero>()
                };
            }

            var statistics = new HeroStatistics
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
                        Percentage = (g.Count() * 100.0) / heroes.Count
                    }).ToList(),
                LowHpHeroes = GetHeroesWithLowHp(50)
            };

            return statistics;
        }

        // Методы для работы с контекстом (если нужны для обратной совместимости)
        public List<Species> GetAllSpeciesFromContext()
        {
            return _context.Species.OrderBy(s => s.Name).ToList();
        }

        public void AddSpeciesToContext(string name, string description)
        {
            var species = new Species { Name = name, Description = description };
            _context.Species.Add(species);
            _context.SaveChanges();
        }

        public Species GetSpeciesByIdFromContext(int id)
        {
            return _context.Species.FirstOrDefault(s => s.Id == id);
        }
    }

        public HeroStatistics GetStatistics()
        {
            var heroes = _heroRepository.ReadAll().ToList();

            if (!heroes.Any())
            {
                return new HeroStatistics
                {
                    TotalHeroes = 0,
                    AverageStrength = 0,
                    AverageHp = 0,
                    MaxStrength = 0,
                    MinHp = 0,
                    SpeciesStats = new List<SpeciesStat>(),
                    DamageTypeStats = new List<DamageTypeStat>(),
                    GenderStats = new List<GenderStat>(),
                    LowHpHeroes = new List<Hero>()
                };
            }

            // Группировка по расам
            var speciesStats = heroes
                .Where(h => h.Species != null)
                .GroupBy(h => h.Species.Name)
                .Select(g => new SpeciesStat
                {
                    Species = g.Key,
                    Count = g.Count(),
                    AvgStrength = g.Average(h => h.Strange),
                    AvgHp = g.Average(h => h.Hp)
                }).ToList();

            // Группировка по типу урона
            var damageStats = heroes
                .GroupBy(h => h.TypeOfDamage)
                .Select(g => new DamageTypeStat
                {
                    DamageType = g.Key,
                    Count = g.Count(),
                    TotalStrength = g.Sum(h => h.Strange)
                }).ToList();

            // Группировка по гендеру
            var genderStats = heroes
                .GroupBy(h => h.Genre)
                .Select(g => new GenderStat
                {
                    Gender = g.Key,
                    Count = g.Count(),
                    Percentage = (g.Count() * 100.0) / heroes.Count
                }).ToList();

            return new HeroStatistics
            {
                TotalHeroes = heroes.Count,
                AverageStrength = heroes.Average(h => h.Strange),
                AverageHp = heroes.Average(h => h.Hp),
                MaxStrength = heroes.Max(h => h.Strange),
                MinHp = heroes.Min(h => h.Hp),
                SpeciesStats = speciesStats,
                DamageTypeStats = damageStats,
                GenderStats = genderStats,
                LowHpHeroes = GetHeroesWithLowHp(50)
            };
        }
    }