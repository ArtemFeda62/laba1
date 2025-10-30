using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public interface IRepository<T> where T : IDomainObject
    {
        void Add(T entity);//добавить запись
        void Delete(int id);//удалить запись
        IEnumerable<T> ReadAll();//получить все записи
        T ReadById(int id);//получить запись
        void Update(T entity);//обновить запись
    }
}//стандартные операции crud