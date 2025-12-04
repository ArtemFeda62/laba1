using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Model.Domain;

namespace DataAccessLayer.Dapper
{
    public class DapperHeroRepository : IHeroRepository
    {
        private readonly string _connectionString =
            @"Data Source=(localdb)\MSSQLLocalDB;
              AttachDbFilename=C:\Users\79082\source\repos\laba1\Model.Domain\HeroDatabase.mdf;
              Integrated Security=True";

        private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

        public void Add(Model.Domain.Hero entity)
        {
            var sql = @"INSERT INTO Heroes (Name, SpeciesId, Genre, Strange, Hp, TypeOfDamage) 
                       VALUES (@Name, @SpeciesId, @Genre, @Strange, @Hp, @TypeOfDamage)";
            using (var connection = CreateConnection())
            {
                connection.Execute(sql, entity);
            }
        }

        public IEnumerable<Model.Domain.Hero> ReadAll()
        {
            var sql = @"
                SELECT h.*, s.Id as SpeciesId, s.Name as SpeciesName, s.Description
                FROM Heroes h 
                INNER JOIN Species s ON h.SpeciesId = s.Id";

            using (var connection = CreateConnection())
            {
                return connection.Query<Model.Domain.Hero, Model.Domain.Species, Model.Domain.Hero>(sql,
                    (hero, species) =>
                    {
                        hero.Species = species;
                        hero.SpeciesId = species.Id;
                        return hero;
                    },
                    splitOn: "SpeciesId");
            }
        }

        public (IEnumerable<Model.Domain.Hero> heroes, int totalCount) ReadAllWithPagination(int pageNumber, int pageSize)
        {
            var sql = @"
                SELECT 
                    h.*, 
                    s.Id as SpeciesId, 
                    s.Name as SpeciesName, 
                    s.Description
                FROM Heroes h 
                INNER JOIN Species s ON h.SpeciesId = s.Id
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
                    var heroes = multi.Read<Model.Domain.Hero, Model.Domain.Species, Model.Domain.Hero>(
                        (hero, species) =>
                        {
                            hero.Species = species;
                            hero.SpeciesId = species.Id;
                            return hero;
                        },
                        splitOn: "SpeciesId"
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

        public Model.Domain.Hero ReadById(int id)
        {
            var sql = @"
                SELECT h.*, s.Id as SpeciesId, s.Name as SpeciesName, s.Description
                FROM Heroes h 
                INNER JOIN Species s ON h.SpeciesId = s.Id 
                WHERE h.Id = @Id";

            using (var connection = CreateConnection())
            {
                return connection.Query<Model.Domain.Hero, Model.Domain.Species, Model.Domain.Hero>(sql,
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

        public void Update(Model.Domain.Hero entity)
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