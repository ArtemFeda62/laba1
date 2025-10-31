using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using DataAccessLayer.Dapper;
using ЛАБА1;

namespace DataAccessLayer.Dapper
{
    public class DapperRepository<T> : IRepository<T> where T : Hero, IDomainObject, new()
    {
        //строчка для подключение к базе данных
        private readonly string _connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=HeroDatabase;Integrated Security=True";
        //метод чтобы подключится к этой базе
        private IDbConnection CreateConnection() => new SqlConnection(_connectionString);
        public void Add(T entity)
        {
            //sql запрос данных с базы
            var sql = @"INSERT INTO Heroes (Name, Species, Genre, Strange, Hp, TypeOfDamage) 
                       VALUES (@Name, @Species, @Genre, @Strange, @Hp, @TypeOfDamage)";
            using (var connection = CreateConnection())//подключение к базе, после чего подключение будет закрыто
            {
                connection.Execute(sql, entity);//благодаря этому методу мы маппим свойства объекта на парраметры entity
            }
        }
        public void Delete(int id)
        {
            var sql = "DELETE FROM Heroes WHERE Id = @Id";//удаление по айди
            using (var connection = CreateConnection())
            {
                connection.Execute(sql, new { Id = id });//маппим @id на свойство id 
            }
        }
        public IEnumerable<T> ReadAll()
        {
            var sql = "SELECT * FROM Heroes";//возвращает все столбцы
            using (var connection = CreateConnection())
            {
                return connection.Query<T>(sql);//этот метод для Select зарпосов,маппит все строчки на объекты Hero
            }
        }
        public T ReadById(int id)
        {
            var sql = "SELECT * FROM Heroes WHERE Id = @Id";
            using (var connection = CreateConnection())
            {
                return connection.QueryFirstOrDefault<T>(sql, new { Id = id });//вовзращает первую строку по айдишке
            }
        }
        public void Update(T entity)
        {
            //устанавливает новые значения свойств при каком-то условии
            var sql = @"UPDATE Heroes SET Name = @Name, Species = @Species, Genre = @Genre, 
                       Strange = @Strange, Hp = @Hp, TypeOfDamage = @TypeOfDamage 
                       WHERE Id = @Id";
            using (var connection = CreateConnection())
            {
                connection.Execute(sql, entity);//меняет все свойства на новые
            }
        }
    }
}