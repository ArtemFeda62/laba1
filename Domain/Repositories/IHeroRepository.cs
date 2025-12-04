using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IHeroRepository : IRepository<Hero>
    {
        (IEnumerable<Hero> heroes, int totalCount) GetWithPagination(int pageNumber, int pageSize);
        IEnumerable<Hero> GetBySpeciesId(int speciesId);
        IEnumerable<Hero> FindByName(string name);
        IEnumerable<Hero> GetWithLowHp(double maxHp);
        IEnumerable<Hero> GetStrongest(int count);
        Dictionary<string, List<Hero>> GroupBySpecies();
        Dictionary<string, List<Hero>> GroupByDamageType();
    }
}
