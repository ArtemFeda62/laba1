using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Shared.Interfases
{
    public interface IRepository<T> where T : IDomainObject
    {
        void Add(T entity);
        void Delete(int id);
        IEnumerable<T> ReadAll();
        T ReadById(int id);
        void Update(T entity);
    }
}
