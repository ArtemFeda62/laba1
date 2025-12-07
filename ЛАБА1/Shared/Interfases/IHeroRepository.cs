using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Interfases
{
    public interface IHeroRepository : IRepository<Hero>
    {
        (IEnumerable<Hero> heroes, int totalCount) ReadAllWithPagination(int pageNumber, int pageSize);
    }
}