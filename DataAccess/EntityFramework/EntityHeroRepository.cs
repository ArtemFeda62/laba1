using Domain;
using Domain.Models;
using Domain.Repositories;
using System.Collections.Generic;
using System.Linq;
namespace DataAccess.EntityFramework
{
    public class EntityHeroRepository : IHeroRepository
    {
        private readonly HeroContext _context;

        public EntityHeroRepository()
        {
            _context = new HeroContext();
        }

        public void Add(Hero entity)
        {
            _context.Heroes.Add(entity);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var hero = _context.Heroes.FirstOrDefault(h => h.Id == id);
            if (hero != null)
            {
                _context.Heroes.Remove(hero);
                _context.SaveChanges();
            }
        }

        public IEnumerable<Hero> ReadAll()
        {
            return _context.Heroes.Include("Species").ToList();
        }

        public Hero ReadById(int id)
        {
            return _context.Heroes.Include("Species").FirstOrDefault(h => h.Id == id);
        }

        public void Update(Hero entity)
        {
            var existingHero = _context.Heroes.Find(entity.Id);
            if (existingHero != null)
            {
                _context.Entry(existingHero).CurrentValues.SetValues(entity);
                _context.SaveChanges();
            }
        }

        public (IEnumerable<Hero> heroes, int totalCount) ReadAllWithPagination(int pageNumber, int pageSize)
        {
            var allHeroes = ReadAll().ToList();
            var pagedHeroes = allHeroes
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return (pagedHeroes, allHeroes.Count);
        }
    }
}