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
        private readonly string _connectionString =
            @"Data Source=(localdb)\MSSQLLocalDB;
              AttachDbFilename=C:\Users\79082\source\repos\laba1\ЛАБА1\HeroDatabase.mdf;
              Integrated Security=True";

        private IDbConnection CreateConnection() => new SqlConnection(_connectionString);
        public IEnumerable<Species> GetAllSpecies()
        {
            var sql = "SELECT * FROM Species";
            using (var connection = CreateConnection())
            {
                return connection.Query<Species>(sql);
            }
        }
        public void Add(T entity)
        {
            var sql = @"INSERT INTO Heroes (Name, SpeciesId, Genre, Strange, Hp, TypeOfDamage) 
                       VALUES (@Name, @SpeciesId, @Genre, @Strange, @Hp, @TypeOfDamage)";
            using (var connection = CreateConnection())
            {
                connection.Execute(sql, entity);
            }
        }
        public IEnumerable<T> ReadAll()
        {
            var sql = @"
                SELECT h.*, s.Id as SpeciesId, s.Name as SpeciesName, s.Description
                FROM Heroes h 
                INNER JOIN Species s ON h.SpeciesId = s.Id";

            using (var connection = CreateConnection())
            {
                return connection.Query<T, Species, T>(sql,
                    (hero, species) =>
                    {
                        hero.Species = species;
                        hero.SpeciesId = species.Id;
                        return hero;
                    },
                    splitOn: "SpeciesId");
            }
        }

        public void Delete(int id)
        {
            var sql = "DELETE FROM Heroes WHERE Id = @Id";
            using (var connection = CreateConnection())
            {
                connection.Execute(sql, new { Id = id });
            }
        }
        public T ReadById(int id)
        {
            var sql = @"
                SELECT h.*, s.Id as SpeciesId, s.Name as SpeciesName, s.Description
                FROM Heroes h 
                INNER JOIN Species s ON h.SpeciesId = s.Id 
                WHERE h.Id = @Id";

            using (var connection = CreateConnection())
            {
                return connection.Query<T, Species, T>(sql,
                    (hero, species) =>
                    {
                        hero.Species = species;
                        hero.SpeciesId = species.Id;
                        return hero;
                    },
                    new { Id = id },
                    splitOn: "SpeciesId").FirstOrDefault();
            }
        }
        public void Update(T entity)
        {
            var sql = @"UPDATE Heroes SET Name = @Name, SpeciesId = @SpeciesId, Genre = @Genre, 
                       Strange = @Strange, Hp = @Hp, TypeOfDamage = @TypeOfDamage 
                       WHERE Id = @Id";
            using (var connection = CreateConnection())
            {
                connection.Execute(sql, entity);
            }
        }
    }
}