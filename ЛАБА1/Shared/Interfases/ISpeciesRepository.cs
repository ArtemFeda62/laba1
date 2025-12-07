using Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Interfases
{
    public interface ISpeciesRepository : IRepository<Species>
    {
        IEnumerable<Species> GetAllOrderedByName();
    }
}
