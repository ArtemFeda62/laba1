using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ЛАБА1;

namespace DataAccessLayer
{
    public interface IRepository<T> where T : IDomainObject
    {
        void Add(T entity);
        void Delete(int id);
        IEnumerable<T> ReadAll();
        T ReadById(int id);
        void Update(T entity);
    }

    // Специализированный интерфейс для героев
    public interface IHeroRepository : IRepository<Hero>
    {
        (IEnumerable<Hero> heroes, int totalCount) ReadAllWithPagination(int pageNumber, int pageSize);
    }

    // Специализированный интерфейс для рас
    public interface ISpeciesRepository : IRepository<Species>
    {
        IEnumerable<Species> GetAllOrderedByName();
    }
}
//стандартные операции crud