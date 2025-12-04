using Domain.Models;
using System;
using System.Collections.Generic;

namespace BusinessLogic.Services
{
    public interface IHeroLogicService
    {
        (List<Hero> heroes, int totalCount) GetHeroesWithDapperPagination(int pageNumber, int pageSize);
        List<Hero> GetHeroesWithPagination(int pageNumber, int pageSize);
        int GetTotalHeroesCount();
        void CreateHero(string name, int speciesId, string genre, int strange, string typeofdamage, double hp);
        Hero GetHero(int id);
        List<Hero> GetAllHeroes();
        void UpdateHero(Hero hero);
        void DeleteHero(int id);
        void ApplyDamage(int id, double damage);
        List<Species> GetAllSpecies();
        void AddSpecies(string name, string description);
        Species GetSpeciesById(int id);
        Species GetSpeciesByName(string name);
        void UpdateSpecies(Species species);
        void DeleteSpecies(int id);
        List<string> GetAvailableSpeciesNames();
        Dictionary<string, List<Hero>> GroupHeroesBySpecies();
        Dictionary<string, List<Hero>> GroupHeroesByDamageType();
        List<Hero> FindHeroesByName(string name);
        List<Hero> GetHeroesWithLowHp(double maxHp);
        List<Hero> GetStrongestHeroes(int count);
        HeroStatistics GetStatistics();
    }
}
