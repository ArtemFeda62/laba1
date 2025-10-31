using System.Collections.Generic;
using System.Linq;
using ЛАБА1;

namespace DataAccessLayer.EntityFramework
{
    public class EntityRepository<T> : IRepository<T> where T : Hero, IDomainObject, new()
    {
        private readonly HeroContext _context;
        public EntityRepository()
        {//создание контекста БД
            _context = new HeroContext();
        }
        public void Add(T entity)
        {//добавление в бд и сохранение
            _context.Heroes.Add(entity);
            _context.SaveChanges();
        }
        public void Delete(int id)
        {//удаление по айдишке
            var hero = _context.Heroes.FirstOrDefault(h => h.Id == id);
            if (hero != null)
            {
                _context.Heroes.Remove(hero);
                _context.SaveChanges();
            }
        }
        public IEnumerable<T> ReadAll()
        {//делаем список из dbset
            return _context.Heroes.ToList() as IEnumerable<T>;
        }
        public T ReadById(int id)
        {//вовзращаем объект по айди
            return _context.Heroes.FirstOrDefault(h => h.Id == id) as T;
        }
        public void Update(T entity)
        {//обновление объекта
            var ent = _context.Heroes.Find(entity.Id);
            if (ent != null)
            {
                _context.Entry(ent).CurrentValues.SetValues(entity);
                _context.SaveChanges();
            }
        }
    }
}