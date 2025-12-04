using System.Collections.Generic;

namespace DataAccessLayer
{
    public interface IDomainObject
    {
        int Id { get; set; }
    }

    public interface IRepository<T> where T : IDomainObject
    {
        void Add(T entity);
        void Delete(int id);
        IEnumerable<T> ReadAll();
        T ReadById(int id);
        void Update(T entity);
    }

    public interface IHeroRepository : IRepository<Model.Domain.Hero>
    {
        (IEnumerable<Model.Domain.Hero> heroes, int totalCount) ReadAllWithPagination(int pageNumber, int pageSize);
    }

    public interface ISpeciesRepository : IRepository<Model.Domain.Species>
    {
        IEnumerable<Model.Domain.Species> GetAllOrderedByName();
    }
}