using Dapper;
using Shared;
using Shared.Domain;
using Shared.Interfases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Dapper
{
    public class DapperHeroRepository : IHeroRepository
    {
        private readonly string _connectionString =
            @"Data Source=(localdb)\MSSQLLocalDB;
              AttachDbFilename=C:\Users\79082\source\repos\laba1\HeroDatabase.mdf;
              Integrated Security=True";

        private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

        public void Add(Hero entity)
        {
            var sql = @"INSERT INTO Heroes (Name, SpeciesId, Genre, Strange, Hp, TypeOfDamage) 
                       VALUES (@Name, @SpeciesId, @Genre, @Strange, @Hp, @TypeOfDamage)";
            using (var connection = CreateConnection())
            {
                connection.Execute(sql, entity);
            }
        }

        public IEnumerable<Hero> ReadAll()
        {
            var sql = @"
        SELECT 
            h.Id, h.Name, h.SpeciesId, h.Genre, h.Strange, h.Hp, h.TypeOfDamage,
            s.Id, s.Name, s.Description
        FROM Heroes h 
        LEFT JOIN Species s ON h.SpeciesId = s.Id";  

            using (var connection = CreateConnection())
            {
                return connection.Query<Hero, Species, Hero>(sql,
                    (hero, species) =>
                    {
                        hero.Species = species;  
                        return hero;
                    },
                    splitOn: "Id");
            }
        }

        public (IEnumerable<Hero> heroes, int totalCount) ReadAllWithPagination(int pageNumber, int pageSize)
        {
            var sql = @"
        SELECT 
            h.Id, h.Name, h.SpeciesId, h.Genre, h.Strange, h.Hp, h.TypeOfDamage,
            s.Id, s.Name, s.Description
        FROM Heroes h 
        LEFT JOIN Species s ON h.SpeciesId = s.Id
        ORDER BY h.Id
        OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;

        SELECT COUNT(*) FROM Heroes;";

            using (var connection = CreateConnection())
            {
                using (var multi = connection.QueryMultiple(sql, new
                {
                    Offset = (pageNumber - 1) * pageSize,
                    PageSize = pageSize
                }))
                {
                    var heroes = multi.Read<Hero, Species, Hero>(
                        (hero, species) =>
                        {
                            hero.Species = species;  
                            return hero;
                        },
                        splitOn: "Id"  
                    ).ToList();

                    var totalCount = multi.ReadSingle<int>();

                    return (heroes, totalCount);
                }
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

        public Hero ReadById(int id)
        {
            var sql = @"
                SELECT h.*, s.Id as SpeciesId, s.Name as SpeciesName, s.Description
                FROM Heroes h 
                INNER JOIN Species s ON h.SpeciesId = s.Id 
                WHERE h.Id = @Id";

            using (var connection = CreateConnection())
            {
                return connection.Query<Hero, Species, Hero>(sql,
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

        public void Update(Hero entity)
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
