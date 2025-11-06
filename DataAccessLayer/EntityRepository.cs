using System.Collections.Generic;
using System.Linq;
using ЛАБА1;

namespace DataAccessLayer.EntityFramework
{
    public class EntityRepository<T> : IRepository<T> where T : Hero, IDomainObject, new()
    {
        private readonly HeroContext _context;
        public EntityRepository()
        {
            _context = new HeroContext();
        }

        public void Add(T entity)
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

        public IEnumerable<T> ReadAll()
        {
            return _context.Heroes.Include("Species").ToList() as IEnumerable<T>;
        }

        public T ReadById(int id)
        {
            return _context.Heroes.Include("Species").FirstOrDefault(h => h.Id == id) as T;
        }
        public void Update(T entity)
        {
            var existingHero = _context.Heroes.Find(entity.Id);
            if (existingHero != null)
            {
                _context.Entry(existingHero).CurrentValues.SetValues(entity);
                _context.SaveChanges();
            }
        }
    }
}